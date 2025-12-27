using ArbitrageAgent.Core.Models;
using SQLite;

namespace ArbitrageAgent.Core.Infrastructure
{
    public class AppDbContext
    {
        private readonly SQLiteAsyncConnection _db;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly Task _initializationTask;

        public AppDbContext(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _initializationTask = InitializeTables();
        }
        public async Task InitializeTables()
        {
            await ExecuteAction(() => _db.CreateTablesAsync<Asset, Exchange, Settings>());
        }
        public async Task<T> ExecuteAction<T>(Func<Task<T>> action)
        {
            await _initializationTask;
            await _semaphore.WaitAsync();

            try
            {
                return await action();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task ResetAsync()
        {
            await ExecuteAction(() => _db.DeleteAllAsync<Asset>());
            await ExecuteAction(() => _db.DeleteAllAsync<Exchange>());
            await ExecuteAction(() => _db.DeleteAllAsync<Settings>());
        }

        public async Task<List<Asset>> GetAssetsAsync() => await ExecuteAction(() => _db.Table<Asset>().ToListAsync());
        public async Task<List<Exchange>> GetExchangesAsync() => await ExecuteAction(() => _db.Table<Exchange>().ToListAsync());
        public async Task<List<Settings>> GetSettingsAsync() => await ExecuteAction(() => _db.Table<Settings>().ToListAsync());
        public async Task<int> AddAsync<T>(T item) => await ExecuteAction(() => _db.InsertAsync(item));
        public async Task<int> SaveAsync<T>(T item) => await ExecuteAction(() => _db.InsertOrReplaceAsync(item));
        public async Task<int> DeleteAsync<T>(T item) => await ExecuteAction(() => _db.DeleteAsync(item));
    }

}
