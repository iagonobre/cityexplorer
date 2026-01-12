using Explorer.Models;
using Refit;
using System.Globalization;
using System.Net.Http.Headers;

namespace Explorer.Services;

public class PlacesService
{
    private readonly INominatimApi _api;

    public PlacesService()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://nominatim.openstreetmap.org")
        };

        httpClient.DefaultRequestHeaders.UserAgent.Clear();
        httpClient.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("ExplorerApp", "1.0")
        );

        _api = RestService.For<INominatimApi>(httpClient);
    }

    public async Task<List<Place>> SearchNearbyAsync(
        double userLatitude,
        double userLongitude,
        string query,
        int limit = 20
    )
    {
        var latOffset = 0.05;
        var lonOffset = 0.05;

        var left = userLongitude - lonOffset;
        var right = userLongitude + lonOffset;
        var top = userLatitude + latOffset;
        var bottom = userLatitude - latOffset;

        var viewbox = $"{left},{top},{right},{bottom}";

        var dtoList = await _api.SearchAsync(
            query: query,
            format: "json",
            limit: limit,
            viewbox: viewbox,
            bounded: 1
        );

        var places = new List<Place>();

        foreach (var dto in dtoList)
        {
            if (!double.TryParse(dto.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat))
                continue;

            if (!double.TryParse(dto.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
                continue;

            var distanceKm = CalculateDistanceKm(
                userLatitude,
                userLongitude,
                lat,
                lon
            );

            places.Add(new Place
            {
                Name = ExtractName(dto.DisplayName),
                Address = dto.DisplayName,
                Latitude = lat,
                Longitude = lon,
                DistanceKm = distanceKm
            });
        }

        return places
            .OrderBy(p => p.DistanceKm)
            .ToList();
    }

    private static double CalculateDistanceKm(
        double lat1,
        double lon1,
        double lat2,
        double lon2
    )
    {
        const double R = 6371;

        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);

        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(DegreesToRadians(lat1)) *
            Math.Cos(DegreesToRadians(lat2)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double DegreesToRadians(double degrees)
        => degrees * (Math.PI / 180);

    private static string ExtractName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            return string.Empty;

        return displayName.Split(',')[0];
    }
}
