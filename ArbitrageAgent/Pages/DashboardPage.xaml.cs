using ArbitrageAgent.ViewModel;
using ArbitrageAgent.ViewModel.Models;
using ArbitrageAgent.Views;

namespace ArbitrageAgent.Pages;

public partial class DashboardPage : ContentPage
{
    private GraphDrawable _graphDrawable;
    public DashboardPage(DashboardViewModel dashboardVM, WeightedRouteViewModel routeVM)
    {
        InitializeComponent();

        BindingContext = routeVM;

        _graphDrawable = new GraphDrawable(routeVM);
        _graphDrawable.InvalidateRequested += (s, e) => GraphView.Invalidate();

        GraphView.Drawable = _graphDrawable;
    }

    private async void OnGoToSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}