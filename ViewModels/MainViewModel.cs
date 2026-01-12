using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Explorer.Models;
using Explorer.Services;
using System.Collections.ObjectModel;

namespace Explorer.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly LocationService _locationService;
    private readonly PlacesService _placesService;

    private const string DefaultCategory = "restaurant";

    private readonly List<Place> _allPlaces = new();

    public ObservableCollection<Place> Places { get; } = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string detectedLocationInfo = string.Empty;

    [ObservableProperty]
    private string emptyMessage = string.Empty;

    public MainViewModel()
    {
        _locationService = new LocationService();
        _placesService = new PlacesService();

        LoadPlacesCommand.Execute(null);
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyLocalFilter();
    }

    [RelayCommand]
    public async Task LoadPlacesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            EmptyMessage = string.Empty;

            Places.Clear();
            _allPlaces.Clear();

            var (location, locationName) =
                await _locationService.GetCurrentLocationWithNameAsync();

            if (location == null)
            {
                EmptyMessage = "Location not available";
                return;
            }

            DetectedLocationInfo = locationName;

            var results = await _placesService.SearchNearbyAsync(
                location.Latitude,
                location.Longitude,
                DefaultCategory
            );

            _allPlaces.AddRange(results);

            ApplyLocalFilter();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyLocalFilter()
    {
        var q = (SearchText ?? string.Empty).Trim();

        IEnumerable<Place> filtered = _allPlaces;

        if (!string.IsNullOrWhiteSpace(q))
        {
            filtered = _allPlaces.Where(p =>
                (p.Name?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Address?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
            );
        }

        Places.Clear();
        foreach (var place in filtered)
            Places.Add(place);

        EmptyMessage = Places.Count == 0
            ? "No results found"
            : string.Empty;
    }

}
