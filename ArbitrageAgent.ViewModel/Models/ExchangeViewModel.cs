using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Models;
using ArbitrageAgent.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.ViewModel.Models
{
    public class ExchangeViewModel : BaseNotify
    {
        private readonly IDataRepository _dataRepository;
        private readonly Exchange _exchange;
        public int Id { get => _exchange.Id; }
        public string Name { get => _exchange.Name; }

        private decimal _tradeFee;
        public decimal TradeFee
        {
            get => _tradeFee;
            set
            {
                _exchange.TradeFee = value;
                SetProperty(ref _tradeFee, value);
                commitChanges();
            }
        }

        private decimal _transferFee;
        public decimal TransferFee
        {
            get => _transferFee;
            set
            {
                _exchange.TransferFee = value;
                SetProperty(ref _transferFee, value);
                commitChanges();
            }
        }

        private bool _enabled;
        public bool Enable
        {
            get => _enabled;
            set 
            {
                _exchange.Enabled = value;
                SetProperty(ref _enabled, value);
                commitChanges();
            }
        }
        public ObservableCollection<AssetViewModel> Assets { get; set; } = new ObservableCollection<AssetViewModel>();
        public ExchangeViewModel(IDataRepository dataRepository, Exchange exchange)
        {
            _dataRepository = dataRepository;
            _exchange = exchange;
            _enabled = exchange.Enabled;
            _tradeFee = exchange.TradeFee;
            _transferFee = exchange.TransferFee;
        }

        private async Task commitChanges()
        {
            await _dataRepository.UpdateExchange(_exchange);
        }

        internal void InitializeAssets(IEnumerable<Asset> assets)
        {
            foreach (var asset in assets)
            {
                AssetViewModel vm = new AssetViewModel(_dataRepository, asset);

                Assets.Add(vm);
            }
        }
    }
}
