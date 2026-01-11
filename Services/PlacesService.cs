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
        double latitude,
        double longitude,
        string query,
        int limit = 20
    )
    {
        var dtoList = await _api.SearchAsync(
            query: query,
            format: "json",
            limit: limit,
            latitude: latitude,
            longitude: longitude
        );

        var places = new List<Place>();

        foreach (var dto in dtoList)
        {
            if (!double.TryParse(dto.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat))
                continue;

            if (!double.TryParse(dto.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
                continue;

            var place = new Place
            {
                Name = ExtractName(dto.DisplayName),
                Address = dto.DisplayName,
                Type = dto.Type,
                Latitude = lat,
                Longitude = lon
            };

            places.Add(place);
        }

        return places;
    }

    private string ExtractName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            return string.Empty;

        var parts = displayName.Split(',');
        return parts.Length > 0 ? parts[0] : displayName;
    }
}