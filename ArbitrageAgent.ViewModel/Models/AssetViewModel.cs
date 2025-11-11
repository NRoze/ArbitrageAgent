using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Models;
using ArbitrageAgent.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.ViewModel.Models
{
    public class AssetViewModel : BaseNotify
    {
        private readonly IDataRepository _dataRepository;
        private readonly Asset _asset;
        public int Id { get => _asset.Id; }
        public int ExchangeId { get => _asset.ExchangeId; }
        public string Fsym { get => _asset.Fsym; }
        public string Tsym { get => _asset.Tsym; }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                _asset.Price = value;
                SetProperty(ref _price, value);
                commitChanges();
            }
        }

        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _asset.Enabled = value;
                SetProperty(ref _enabled, value);
                commitChanges();
            }
        }
        private async Task commitChanges()
        {
            await _dataRepository.UpdateAsset(_asset);
        }

        public AssetViewModel(IDataRepository dataRepository, Asset asset)
        {
            _dataRepository = dataRepository;
            _asset = asset;
            _price = asset.Price;
            _enabled = asset.Enabled;
        }
    }
}
