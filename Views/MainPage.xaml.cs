using Explorer.Services;

namespace Explorer;

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
            var service = new PlacesService();

            var places = await service.SearchNearbyAsync(
                latitude: 59.3293,
                longitude: 18.0686,
                query: "restaurant"
            );

            if (places.Any())
            {
                CounterBtn.Text = places.First().Name;
            }
            else
            {
                CounterBtn.Text = "No places found";
            }
        }
        catch (Exception ex)
        {
            CounterBtn.Text = "Error";
            Console.WriteLine(ex);
        }
    }
}