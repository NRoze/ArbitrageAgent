using ArbitrageAgent.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Services
{
    public class MaxWeightedRouteService
    {
        public IEnumerable<(List<AssetNode> Route, decimal ProfitRate)> GetAllRoutes(IEnumerable<AssetNode> i_Nodes, string i_StartingCurrency = "USD")
        {
            List<(List<AssetNode> Route, decimal ProfitRate)> result = new List<(List<AssetNode> Route, decimal ProfitRate)>();
            (List<AssetNode> Route, decimal ProfitRate) currentRoute = (null, 1M);

            if (i_Nodes != null)
            {
                foreach (var node in i_Nodes.Where(x => x.Name == i_StartingCurrency))
                {
                    findRoutes(node, i_StartingCurrency, ref result, currentRoute);
                }
            }

            return result.Where(x => x.ProfitRate > 1.00001M).OrderByDescending(x => x.ProfitRate);
        }

        private void findRoutes(AssetNode i_Node, string i_StartingCurrency,
            ref List<(List<AssetNode> Route, decimal ProfitRate)> routes,
            (List<AssetNode> Route, decimal ProfitRate) currentRoute,
            bool i_WasTransferLink = false)
        {
            bool keepGoing = true, isTranferLink = false;
            decimal feeFactor;

            if (currentRoute.Route == null)
            {
                currentRoute = (new List<AssetNode>() { i_Node }, 1M);
            }
            else
            {
                currentRoute.Route.Add(i_Node);
                if (i_Node.Name == i_StartingCurrency)
                {
                    routes.Add(currentRoute);
                    keepGoing = false;
                }
            }
            if (keepGoing)
            {
                foreach (var linkedNode in i_Node.Links)
                {
                    if (linkedNode.Node.Name == i_StartingCurrency || !currentRoute.Route.Any(x => x == linkedNode.Node))
                    {
                        isTranferLink = (i_Node.Name == linkedNode.Node.Name && i_Node.ExchangeId != linkedNode.Node.ExchangeId);
                        if (i_WasTransferLink && isTranferLink)
                        {
                            continue;
                        }

                        feeFactor = 1 - (isTranferLink ? linkedNode.Node.TransferFee : linkedNode.Node.TradeFee) / 100M;
                        findRoutes(linkedNode.Node, i_StartingCurrency, ref routes,
                            (new List<AssetNode>(currentRoute.Route), currentRoute.ProfitRate * linkedNode.Weight * feeFactor),
                            isTranferLink);
                    }
                }
            }
        }

        public IEnumerable GetTransferLinks(IEnumerable<AssetMetadata> i_Assets)
        {
            List<(AssetMetadata From, AssetMetadata To, string Profit)> result = 
                new List<(AssetMetadata From, AssetMetadata To, string Profit)>();
            decimal profit;

            foreach (var from in i_Assets)
            {
                foreach (var to in i_Assets.Where(x => x.Fsym == from.Fsym &&
                                                        x.Tsym == from.Tsym &&
                                                        x.ExchangeId != from.ExchangeId))
                {
                    profit = to.Price * (1 - to.TransferFee) - from.Price * (1 + from.TransferFee);
                    profit -= from.Price * from.TransferFee;
                    if (profit > 0)
                    {
                        result.Add((from, to, string.Format("{0} ({1:0.0000}%)", profit, profit * 100 / from.Price)));
                    }
                }
            }

            return result.OrderByDescending(x => x.Profit);
        }
        public List<(AssetNode From, AssetNode To, decimal Profit)> GetTransferLinks(IEnumerable<AssetNode> i_Nodes)
        {
            List<(AssetNode From, AssetNode To, decimal Profit)> result = new List<(AssetNode From, AssetNode To, decimal Profit)>();

            foreach (AssetNode from in i_Nodes)
            {
                foreach (var to in from.Links.Where(x => x.Node.Name == from.Name && x.Node.ExchangeId != from.ExchangeId))
                {
                    result.Add((from, to.Node, to.Weight));
                }
            }

            return result;
        }

        public void GetLongestRoute(List<AssetNode> nodes, ref decimal weight, ref List<AssetNode> route)
        {
            decimal tempWeight = 1;
            List<AssetNode> maxRoute = new List<AssetNode>();

            foreach (AssetNode node in nodes.Where(x => x.Name == "USD"))
            {
                getLongestRouteWeight(node, new List<AssetNode> { node }, 1, ref tempWeight, ref maxRoute);
                if (tempWeight > weight)
                {
                    weight = tempWeight;
                    route = maxRoute;
                }

                tempWeight = 1;
            }
        }

        private void getLongestRouteWeight(
            AssetNode currentNode, List<AssetNode> currentRoute, decimal currentWeight, ref decimal maxWeight, ref List<AssetNode> maxRoute)
        {
            if (currentNode.Links.Count == 0 && currentNode.Name == "USD")
            {
                checkEndOfRoute(currentRoute, currentWeight, ref maxWeight, ref maxRoute);
            }
            else
            {
                foreach (var node in currentNode.Links)
                {
                    if (!currentRoute.Contains(node.Node))
                    {
                        getLongestRouteWeight(
                            node.Node, currentRoute.Append(node.Node).ToList(), node.Weight * currentWeight, ref maxWeight, ref maxRoute);
                        if (node.Node.Name == "USD")
                        {
                            checkEndOfRoute(currentRoute, currentWeight, ref maxWeight, ref maxRoute);
                        }
                    }
                    else if (node.Node.Name == "USD")
                    {
                        checkEndOfRoute(currentRoute, currentWeight, ref maxWeight, ref maxRoute);
                    }
                }
            }
        }

        private void checkEndOfRoute(List<AssetNode> currentRoute, decimal currentWeight, ref decimal maxWeight, ref List<AssetNode> maxRoute)
        {
            if (currentWeight > maxWeight)
            {
                maxWeight = currentWeight;
                maxRoute = currentRoute;
            }
        }

    }
}
