using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Models;
using System.Collections.Concurrent;

namespace ArbitrageAgent.Core.Services
{
    public class AssetLinkGraphService : BaseNotify
    {
        private ConcurrentDictionary<(string, int), AssetNode> _nodesMap { get; set; }
        public IEnumerable<AssetNode> Nodes => _nodesMap.Values;

        public IEnumerable<AssetNode> BuildGraph(IEnumerable<AssetMetadata> i_Assets)
        {
            _nodesMap = new();

            populateNodes(i_Assets);
            populateLinks(i_Assets);
            OnPropertyChanged(nameof(Nodes));

            return Nodes;
        }

        private void populateLinks(IEnumerable<AssetMetadata> i_Assets)
        {
            foreach (AssetNode node in Nodes)
            {
                populateNodeLinksTransfers(node, i_Assets);
                populateNodeLinksTrade(node, i_Assets);
            }
        }

        private void populateNodeLinksTransfers(AssetNode i_Node, IEnumerable<AssetMetadata> i_Assets)
        {
            AssetMetadata assetFrom, assetTo;

            foreach (AssetNode node in Nodes.Where(x => x.Name == i_Node.Name && x.ExchangeId != i_Node.ExchangeId))
            {
                assetFrom = i_Assets.FirstOrDefault(x => x.ExchangeId != i_Node.ExchangeId &&
                                                    x.Fsym == i_Node.Name);
                assetTo = i_Assets.FirstOrDefault(x => x.ExchangeId != node.ExchangeId &&
                                                    x.Fsym == node.Name &&
                                                    x.Tsym == assetFrom.Tsym);
                if (assetFrom != null && assetTo != null)
                {
                    i_Node.AddLink((node, assetTo.Price / assetFrom.Price, "Transfer"));
                }
            }
        }

        private void populateNodeLinksTrade(AssetNode i_Node, IEnumerable<AssetMetadata> i_Assets)
        {
            AssetNode node;

            foreach (AssetMetadata asset in i_Assets.Where(x => x.ExchangeId == i_Node.ExchangeId))
            {
                if (i_Node.Name == asset.Fsym)
                {
                    node = Nodes.FirstOrDefault(x => x.ExchangeId == i_Node.ExchangeId && x.Name == asset.Tsym);
                    if (node != null)
                    {
                        i_Node.AddLink((node, asset.Price, "Trade"));
                    }
                }
                if (i_Node.Name == asset.Tsym)
                {
                    node = Nodes.FirstOrDefault(x => x.ExchangeId == i_Node.ExchangeId && x.Name == asset.Fsym);
                    if (node != null)
                    {
                        i_Node.AddLink((node, 1 / asset.Price, "Trade"));
                    }
                }
            }
        }

        private void populateNodes(IEnumerable<AssetMetadata> i_Assets)
        {
            AssetNode nodeF, nodeT;

            foreach (AssetMetadata asset in i_Assets)
            {
                nodeF = new AssetNode(asset, asset.Fsym);
                nodeT = new AssetNode(asset, asset.Tsym);
                addNode(nodeF);
                addNode(nodeT);
            }
        }

        private void addNode(AssetNode node)
        {
            _nodesMap.TryAdd((node.Name, node.ExchangeId), node);
        }
    }
}
