using ArbitrageAgent.Core.Services;

namespace ArbitrageAgent
{
    public partial class App : Application
    {
        private readonly HeartbeatService _heartbeat;
        public App(HeartbeatService heartbeat)
        {
            InitializeComponent();

            _heartbeat = heartbeat;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_heartbeat));
        }
    }
}