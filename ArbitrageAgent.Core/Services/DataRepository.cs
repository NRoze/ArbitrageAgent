using ArbitrageAgent.Core.Infrastructure;
using ArbitrageAgent.Core.Models;

namespace ArbitrageAgent.Core.Services
{
    public class DataRepository : IDataRepository
    {
        private readonly AppDbContext _db;

        public DataRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Exchange>> GetExchanges() => await _db.GetExchangesAsync();
        public async Task AddExchange(Exchange exchange) => await _db.AddAsync(exchange);
        public async Task UpdateExchange(Exchange exchange) => await _db.SaveAsync(exchange);
        public async Task DeleteExchange(Exchange exchange) => await _db.DeleteAsync(exchange);
        public async Task<IEnumerable<Asset>> GetAssets() => await _db.GetAssetsAsync();
        public async Task AddAsset(Asset asset) => await _db.AddAsync(asset);
        public async Task UpdateAsset(Asset asset) => await _db.SaveAsync(asset);
        public async Task DeleteAsset(Asset asset) => await _db.DeleteAsync(asset);

        public async Task<IEnumerable<Settings>> GetSettings() => await _db.GetSettingsAsync();
        public async Task UpdateSettings(Settings settings) => await _db.SaveAsync(settings);
        public async Task ResetAsync() => await _db.ResetAsync();
    }
}
