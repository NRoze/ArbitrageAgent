using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Models
{
    public class Exchange
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TradeFee { get; set; }
        public decimal TransferFee { get; set; }
        public bool Enabled { get; set; }
    }
}
