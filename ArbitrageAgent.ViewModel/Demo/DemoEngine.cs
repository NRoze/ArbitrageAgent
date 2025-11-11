using ArbitrageAgent.Core.Models;
using ArbitrageAgent.Core.Services;
using ArbitrageAgent.ViewModel.Models;
using System.Collections.ObjectModel;

namespace ArbitrageAgent.ViewModel.Demo
{
    public class DemoEngine(IDataRepository dataRepository)
    {
        private readonly IDataRepository _dataRepo = dataRepository;

        public async Task GenerateDemo()
        {
            await _dataRepo.ResetAsync();

            var exchanges = new List<Exchange>
            {
                new Exchange { Name = "Binance", TradeFee = 0.001m, TransferFee = 0.002m },
                new Exchange { Name = "Coinbase", TradeFee = 0.002m, TransferFee = 0.002m  },
                new Exchange { Name = "Bitsamp", TradeFee = 0.001m, TransferFee = 0.003m  }
            };

            await _dataRepo.UpdateSettings(new Settings { Key = "HeartbeatInterval", Value = "6000" });
            foreach (var exchange in exchanges)
            {
                await _dataRepo.AddExchange(exchange);
                Asset btc = new Asset { ExchangeId = exchange.Id, Fsym = "BTC", Tsym = "USD", Enabled = true, Price = 100000m };
                Asset eth = new Asset { ExchangeId = exchange.Id, Fsym = "ETH", Tsym = "USD", Enabled = true, Price = 4000m };
                Asset sol = new Asset { ExchangeId = exchange.Id, Fsym = "SOL", Tsym = "USD", Enabled = true, Price = 200m };
                //Asset doge = new Asset { ExchangeId = exchange.Id, Fsym = "XRP", Tsym = "USD", Enabled = true, Price = 3m };

                await _dataRepo.AddAsset(btc);
                await _dataRepo.AddAsset(eth);
                await _dataRepo.AddAsset(sol);
                //await _dataRepo.AddAsset(doge);
            }
        }

        internal void ManipulateData(ObservableCollection<ExchangeViewModel> exchanges)
        {
            foreach (var ex in exchanges)
            {
                foreach (var asset in ex.Assets)
                {
                    // Simple random walk for price simulation
                    var rand = new Random();
                    var changePercent = (decimal)(rand.NextDouble() * 0.0002 - 0.0001); // -.01% to +.01%
                    asset.Price += asset.Price * changePercent;
                }
            }
        }
    }
}
