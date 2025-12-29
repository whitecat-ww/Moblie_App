using SQLite;
using System.IO;
using System.Threading.Tasks;

namespace SosTrackerApp
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _connection;
        private const string DbName = "SosTracker.db3";

        public DatabaseService()
        {
            // Set path for Android/Windows/iOS
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
            _connection = new SQLiteAsyncConnection(dbPath);

            // Create Table (Safe to call every time)
            _connection.CreateTableAsync<TripLog>().Wait();
        }

        public async Task AddLogAsync(TripLog log)
        {
            await _connection.InsertAsync(log);
        }
    }
}