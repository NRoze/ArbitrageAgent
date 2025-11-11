using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Services
{
    public class HeartbeatService
    {
        private readonly System.Timers.Timer _timer;
        public event Action? Elapsed;
        public double Interval { set => _timer.Interval = value; }

        public HeartbeatService()
        {
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += _timer_Elapsed;
        }

        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Elapsed?.Invoke();
        }
    }
}
