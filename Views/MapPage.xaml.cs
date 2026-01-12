using Explorer.ViewModels;

namespace Explorer.Views;

[QueryProperty(nameof(Latitude), "lat")]
[QueryProperty(nameof(Longitude), "lon")]
[QueryProperty(nameof(Name), "name")]
[QueryProperty(nameof(Address), "address")]
[QueryProperty(nameof(Distance), "distance")]
public partial class MapPage : ContentPage
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Distance { get; set; }

    public MapPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new MapViewModel(
            Name,
            Address,
            Distance,
            Latitude,
            Longitude
        );
    }
}