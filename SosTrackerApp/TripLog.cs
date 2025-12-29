using SQLite; // 引用 SQLite 库
using System;

namespace SosTrackerApp // 确保这里的名字和你项目名字一致
{
    public class TripLog
    {
        // 这是一个主键，自动增长（1, 2, 3...）
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // 存储用户输入的任务 ID (例如 SOS-001)
        public string TripId { get; set; }

        // 存储经纬度字符串 (例如 "2.31, 111.82")
        public string GeoLocation { get; set; }

        // 存储保存数据时的时间
        public DateTime CreatedAt { get; set; }
    }
}