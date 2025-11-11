using ArbitrageAgent.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Services
{
    public interface IDataRepository
    {
        Task<IEnumerable<Exchange>> GetExchanges();
        Task AddExchange(Exchange exchange);
        Task UpdateExchange(Exchange exchange);
        Task DeleteExchange(Exchange exchange);

        Task<IEnumerable<Asset>> GetAssets();
        Task AddAsset(Asset asset);
        Task UpdateAsset(Asset asset);
        Task DeleteAsset(Asset asset);

        Task<IEnumerable<Settings>> GetSettings();
        Task UpdateSettings(Settings asset);

        Task ResetAsync();
    }
}
