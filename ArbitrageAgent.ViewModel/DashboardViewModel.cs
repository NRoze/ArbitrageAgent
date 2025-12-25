using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Models;
using ArbitrageAgent.Core.Services;
using ArbitrageAgent.ViewModel.Demo;
using ArbitrageAgent.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace ArbitrageAgent.ViewModel
{
    public class DashboardViewModel : BaseNotify
    {
        private readonly DemoEngine _demoEngine;
        private readonly IDataRepository _dataRepo;

        public ObservableCollection<ExchangeViewModel> Exchanges { get; } = new ();
        private ExchangeViewModel? _selectedExchange;
        public ExchangeViewModel? SelectedExchange 
        {
            get => _selectedExchange;
            set
            { 
                SetProperty(ref _selectedExchange, value);
            }
        }
        public DashboardViewModel(IDataRepository dataRepository, 
                                    HeartbeatService heartbeatService)
        {
            _dataRepo = dataRepository;
            _demoEngine = new DemoEngine(_dataRepo);

            Task.Run(async () =>
            {
                await Initialize();

                if (Exchanges.Count == 0)
                {
                    // For demo purposes, generate some data
                    await _demoEngine.GenerateDemo();
                    await Initialize();
                }
                heartbeatService.Elapsed += _heartbeatTimer_Elapsed;
            });
        }

        private void _heartbeatTimer_Elapsed()
        {
            _demoEngine.ManipulateData(Exchanges);
            
        }

        public async Task Initialize()
        {
            var exchanges = await _dataRepo.GetExchanges();
            var assets = await _dataRepo.GetAssets();

            foreach (var ex in exchanges)
            {
                ExchangeViewModel viewModel = new (_dataRepo, ex);

                viewModel.InitializeAssets(assets.Where(a => a.ExchangeId == ex.Id));

                Exchanges.Add(viewModel);
            }

            SelectedExchange = Exchanges.FirstOrDefault();
        }
    }
}
