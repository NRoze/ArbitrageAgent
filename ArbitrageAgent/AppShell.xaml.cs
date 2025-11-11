using ArbitrageAgent.Core.Services;
using ArbitrageAgent.Pages;

namespace ArbitrageAgent
{
    public partial class AppShell : Shell
    {
        private readonly HeartbeatService _heartbeat;
        public AppShell(HeartbeatService heartbeat)
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));

            _heartbeat = heartbeat;
            _heartbeat.Start();
        }

        override protected void OnDisappearing()
        {
            base.OnDisappearing();
            _heartbeat.Stop();
        }
    }
}
