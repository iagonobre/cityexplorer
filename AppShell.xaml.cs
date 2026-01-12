using Explorer.Views;

namespace Explorer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("map", typeof(MapPage));

    }
}