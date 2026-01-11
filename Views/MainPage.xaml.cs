using Explorer.Services;

namespace Explorer.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            var locationService = new LocationService();
            var location = await locationService.GetCurrentLocationAsync();

            if (location == null)
            {
                CounterBtn.Text = "Location not available";
                return;
            }

            var placesService = new PlacesService();

            var places = await placesService.SearchNearbyAsync(
                latitude: location.Latitude,
                longitude: location.Longitude,
                query: "restaurant"
            );

            CounterBtn.Text = places.Any()
                ? places.First().Name
                : "No places found";
        }
        catch (Exception ex)
        {
            CounterBtn.Text = ex.GetType().Name;
            await DisplayAlert("Error", ex.ToString(), "OK");
            Console.WriteLine(ex);
        }
    }
}