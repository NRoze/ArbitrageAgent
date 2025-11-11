using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.ViewModel.Models
{
    public class SettingsItemViewModel : BaseNotify
    {
        private readonly Settings _settingsItem;

        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _settingsItem.Key = value;
                SetProperty(ref _key, value);
            }
        }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _settingsItem.Value = value;
                SetProperty(ref _value, value);
            }
        }
        public SettingsItemViewModel(Settings settingsItem)
        {
            _settingsItem = settingsItem;
        }
    }
}
