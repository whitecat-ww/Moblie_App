using SQLite;

public class DatabaseService
{
    // 步骤 1: 定义数据库路径
    private const string DB_NAME = "sos_data.db3";
    private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);

    private SQLiteAsyncConnection _connection;

    public DatabaseService()
    {
        // 步骤 2: 初始化连接
        _connection = new SQLiteAsyncConnection(_dbPath);

        // 步骤 3: 创建表
        _connection.CreateTableAsync<TripLog>().Wait();
    }

    // 步骤 4: CRUD 操作 (插入数据)
    public async Task<int> AddLogAsync(TripLog log)
    {
        return await _connection.InsertAsync(log);
    }
}