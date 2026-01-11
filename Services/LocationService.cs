namespace Explorer.Services;

public class LocationService
{
    public async Task<Location?> GetCurrentLocationAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        if (status != PermissionStatus.Granted)
            return null;

        var location = await Geolocation.GetLastKnownLocationAsync();

        if (location != null)
            return location;

        var request = new GeolocationRequest(
            GeolocationAccuracy.Medium,
            TimeSpan.FromSeconds(10)
        );

        return await Geolocation.GetLocationAsync(request);
    }
}