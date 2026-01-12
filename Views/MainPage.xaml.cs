using Explorer.Models;
using Explorer.ViewModels;

namespace Explorer.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        BindingContext = new MainViewModel();
    }

    private async void OnPlaceSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Place place)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Shell.Current.GoToAsync(
            $"map?" +
            $"lat={place.Latitude}" +
            $"&lon={place.Longitude}" +
            $"&name={Uri.EscapeDataString(place.Name)}" +
            $"&address={Uri.EscapeDataString(place.Address)}" +
            $"&distance={place.DistanceKm}"
        );

    }
}