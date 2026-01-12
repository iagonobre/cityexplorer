using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;

namespace Explorer.ViewModels;

public partial class MapViewModel : ObservableObject
{
    public double Latitude { get; }
    public double Longitude { get; }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private string distanceText;

    public MapViewModel(
        string name,
        string address,
        double distanceKm,
        double latitude,
        double longitude)
    {
        Name = name;
        Address = address;
        DistanceText = $"{distanceKm:F1} km away";
        Latitude = latitude;
        Longitude = longitude;
    }

    [RelayCommand]
    public async Task OpenMapAsync()
    {
        var location = new Location(Latitude, Longitude);

        var options = new MapLaunchOptions
        {
            Name = Name
        };

        await Map.OpenAsync(location, options);
    }
}