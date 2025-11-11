using ArbitrageAgent.Core.Models;
using SQLite;

namespace ArbitrageAgent.Core.Infrastructure
{
    public class AppDbContext
    {
        private readonly SQLiteAsyncConnection _db;

        public AppDbContext(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Asset>().Wait();
            _db.CreateTableAsync<Exchange>().Wait();
            _db.CreateTableAsync<Settings>().Wait();
        }

        public async Task ResetAsync()
        {
            await _db.DeleteAllAsync<Asset>().ConfigureAwait(false);
            await _db.DeleteAllAsync<Exchange>().ConfigureAwait(false);
            await _db.DeleteAllAsync<Settings>().ConfigureAwait(false);
        }

        public async Task<List<Asset>> GetAssetsAsync() => await _db.Table<Asset>().ToListAsync();
        public async Task<List<Exchange>> GetExchangesAsync() => await _db.Table<Exchange>().ToListAsync();
        public async Task<List<Settings>> GetSettingsAsync() => await _db.Table<Settings>().ToListAsync();
        public async Task<int> AddAsync<T>(T item) => await _db.InsertAsync(item);
        public async Task<int> SaveAsync<T>(T item) => await _db.InsertOrReplaceAsync(item);
        public async Task<int> DeleteAsync<T>(T item) => await _db.DeleteAsync(item);
    }

}
