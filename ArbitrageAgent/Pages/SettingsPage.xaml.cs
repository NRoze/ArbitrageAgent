using ArbitrageAgent.ViewModel;

namespace ArbitrageAgent.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
        
        BindingContext = vm;
    }

    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(".."); // navigate back
    }
}