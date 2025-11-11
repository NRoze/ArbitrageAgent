using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Models
{
    public class Settings
    {
        [PrimaryKey]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
