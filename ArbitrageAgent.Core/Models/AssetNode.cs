using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Models
{
    public class AssetNode
    {
        public string Name { get; set; }
        public int ExchangeId { get; set; }
        public string ExchangeName { get; set; }
        public decimal TradeFee { get; set; }
        public decimal TransferFee { get; set; }
        public List<(AssetNode Node, decimal Weight, string Type)> Links { get; } = new List<(AssetNode Node, decimal Weight, string Type)>();
        internal void AddLink((AssetNode, decimal, string) link)
        {
            if (!Links.Any(x => x == link))
            {
                Links.Add(link);
            }
        }
        public AssetNode(AssetMetadata metadata, string nodeName)
        {
            Name = nodeName;
            ExchangeId = metadata.ExchangeId;
            ExchangeName = metadata.ExchangeName;
            TradeFee = metadata.TradeFee;
            TransferFee = metadata.TransferFee;
        }
    }
}
