using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Models
{
    public class Asset
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ExchangeId { get; set; }
        public string Fsym { get; set; }
        public string Tsym { get; set; }
        public decimal Price { get; set; }
        public bool Enabled { get; set; }
    }
}
