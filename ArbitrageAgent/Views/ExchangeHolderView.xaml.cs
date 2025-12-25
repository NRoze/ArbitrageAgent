using ArbitrageAgent.ViewModel.Models;

namespace ArbitrageAgent.Views;

public partial class ExchangeHolderView : ContentView
{
    private readonly ExchangeView _exchangeView = new();
    public ExchangeHolderView()
	{
		InitializeComponent();

        BindingContextChanged += ExchangeHolderView_BindingContextChanged;
	}

    //public void ShowRegisterView()
    //{
    //    ContentPlaceholder.Content = new RegisterExchangeView();
    //}

    private void ExchangeHolderView_BindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is ExchangeViewModel vm)
        {
            _exchangeView.BindingContext = vm;
            ContentPlaceholder.Content = _exchangeView;
        }
        else
        {
            ContentPlaceholder.Content = null;
        }
    }
}