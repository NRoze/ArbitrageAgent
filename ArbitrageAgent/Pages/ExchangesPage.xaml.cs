using ArbitrageAgent.ViewModel;

namespace ArbitrageAgent.Pages;

public partial class ExchangesPage : ContentPage
{
	public ExchangesPage(DashboardViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}