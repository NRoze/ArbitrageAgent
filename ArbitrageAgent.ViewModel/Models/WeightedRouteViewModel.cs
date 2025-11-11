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
    public class WeightedRouteViewModel : BaseNotify
    {
        private readonly MaxWeightedRouteService _routeService;
        private readonly AssetLinkGraphService _graphService;
        private readonly DashboardViewModel _dashboardVM;
        private readonly SynchronizationContext _uiContext = SynchronizationContext.Current!;
        private DateTime _lastUpdate;
        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set
            {
                _lastUpdate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AssetNode> Graph { get; private set; } = new ObservableCollection<AssetNode>();
        public ObservableCollection<(AssetNode From, AssetNode To, decimal Profit)> TransferLinks { get; private set; } =
            new ObservableCollection<(AssetNode From, AssetNode To, decimal Profit)>();
        public ObservableCollection<(List<AssetNode> Route, decimal ProfitRate)> Routes { get; private set; } = 
            new ObservableCollection<(List<AssetNode> Route, decimal ProfitRate)>();

        public WeightedRouteViewModel(AssetLinkGraphService graphService, MaxWeightedRouteService routeService,
                                        DashboardViewModel dashboardVM, HeartbeatService heartbeatService)
        {
            _routeService = routeService;
            _graphService = graphService;
            _dashboardVM = dashboardVM;

            heartbeatService.Elapsed += _heartbeatService_Elapsed;
        }

        private void _heartbeatService_Elapsed()
        {
            List<AssetMetadata> assets = generateMetadata();
            var graph = _graphService.BuildGraph(assets);
            var transferLinks = _routeService.GetTransferLinks(graph);

            _uiContext.Post(_ =>
            {
                Graph.Clear();
                foreach (var node in graph)
                {
                    Graph.Add(node);
                }

                TransferLinks.Clear();
                foreach (var link in transferLinks)
                {
                    TransferLinks.Add(link);
                }

                var routes = _routeService.GetAllRoutes(graph);
                Routes.Clear();
                foreach (var route in routes)
                {
                    Routes.Add(route);
                }
            }, null);
            try
            {
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating WeightedRouteViewModel: {ex.Message}");
            }

            LastUpdate = DateTime.Now;
        }

        private List<AssetMetadata> generateMetadata()
        {
            List<AssetMetadata> assets = new List<AssetMetadata>();

            foreach (var ex in _dashboardVM.Exchanges)
            {
                foreach (var asset in ex.Assets.Where(a => a.Enabled))
                {
                    assets.Add(new AssetMetadata
                    {
                        Id = asset.Id,
                        ExchangeId = asset.ExchangeId,
                        ExchangeName = ex.Name,
                        Price = asset.Price,
                        TradeFee = ex.TradeFee,
                        TransferFee = ex.TransferFee,
                        Fsym = asset.Fsym,
                        Tsym = asset.Tsym
                    });
                }
            }

            return assets;
        }
    }
}
