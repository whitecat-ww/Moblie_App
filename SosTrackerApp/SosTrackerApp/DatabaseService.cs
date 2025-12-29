using SQLite;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage; // 关键引用

namespace SosTrackerApp
{
    public class DatabaseService
    {
        private const string DB_NAME = "sos_mission_data.db3";
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public DatabaseService()
        {
            // 获取手机本地存储路径
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);

            _connection = new SQLiteAsyncConnection(_dbPath);
            _connection.CreateTableAsync<TripLog>().Wait();
        }

        public async Task<int> AddLogAsync(TripLog log)
        {
            return await _connection.InsertAsync(log);
        }
    }
}