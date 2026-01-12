using Refit;
using System.Net.Http.Headers;

namespace Explorer.Services;

public class LocationService
{
    private readonly INominatimReverseApi _reverseApi;

    public LocationService()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://nominatim.openstreetmap.org")
        };

        httpClient.DefaultRequestHeaders.UserAgent.Clear();
        httpClient.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("ExplorerApp", "1.0")
        );

        _reverseApi = RestService.For<INominatimReverseApi>(httpClient);
    }

    public async Task<(Location? location, string locationName)> GetCurrentLocationWithNameAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
            return (null, string.Empty);

        Location? location;

        try
        {
            var request = new GeolocationRequest(
                GeolocationAccuracy.Medium,
                TimeSpan.FromSeconds(10)
            );

            location = await Geolocation.GetLocationAsync(request);
        }
        catch
        {
            return (null, string.Empty);
        }

        if (location == null)
            return (null, string.Empty);

        var reverse = await _reverseApi.ReverseAsync(
            location.Latitude,
            location.Longitude,
            "json"
        );

        var address = reverse.Address;

        var city =
            address.City ??
            address.Town ??
            address.Village ??
            string.Empty;

        var locationName = string.IsNullOrWhiteSpace(city)
            ? address.Country
            : $"{city}, {address.Country}";

        return (location, locationName);
    }

}