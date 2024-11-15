     using SQLite;
     using System.Collections.Generic;
     using System.Threading.Tasks;
     using Health.Models;

namespace Health.Services
     {
    public class HealthDataService
    {
        private readonly SQLiteAsyncConnection _database;

        public HealthDataService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
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
    }
     }
     