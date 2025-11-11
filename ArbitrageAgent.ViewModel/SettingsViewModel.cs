using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Models;
using ArbitrageAgent.Core.Services;
using ArbitrageAgent.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.ViewModel
{
    public class SettingsViewModel : BaseNotify
    {
        private double _heartbeatInterval;
        private readonly HeartbeatService _heartbeatService;
        private readonly IDataRepository _dataRepo;

        public double HeartbeatInterval
        {
            get => _heartbeatInterval;
            set
            {
                _heartbeatService.Interval = value;
                SetProperty(ref _heartbeatInterval, value);
                commitChanges(nameof(HeartbeatInterval), value.ToString());
            }
        }

        public SettingsViewModel(IDataRepository dataRepository, HeartbeatService heartbeatService)
        {
            _heartbeatService = heartbeatService;
            _dataRepo = dataRepository;
        }

        private void parseHeartbeatInterval(Settings? settings)
        {
            if (settings != null && double.TryParse(settings.Value, out double interval))
            {
                HeartbeatInterval = interval;
            }
        }

        public async Task InitializeAsync()
        {
            var settings = await _dataRepo.GetSettings();

            if (settings?.Count() > 0)
            {
                parseHeartbeatInterval(settings.FirstOrDefault(x => x.Key == nameof(HeartbeatInterval)));
            }
        }

        private void commitChanges(string key, string value)
        {
            _dataRepo.UpdateSettings(new Settings
            {
                Key = key,
                Value = value
            });
        }
    }
}
