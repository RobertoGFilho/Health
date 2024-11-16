     using SQLite;
     using System.Collections.Generic;
     using System.Threading.Tasks;
     using Health.Models;

namespace Health.Services
{
    public class HealthDataService
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly string _databasePath;

        public HealthDataService(string dbPath)
        {
            _databasePath = dbPath;
            _database = new SQLiteAsyncConnection(_databasePath);
            _database.CreateTableAsync<HealthData>().Wait();
        }

        public Task<List<HealthData>> GetHealthDataAsync()
        {
            return _database.Table<HealthData>().ToListAsync();
        }

        public Task<int> SaveHealthDataAsync(HealthData data)
        {
            return _database.InsertAsync(data);
        }

        public Task<int> DeleteHealthDataAsync(HealthData healthData)
        {
            return _database.DeleteAsync(healthData);

        }
        public async Task DeleteAllHealthDataAsync()
        {
            await _database.DropTableAsync<HealthData>();
            await _database.CreateTableAsync<HealthData>();
        }
    }
}
     