using SQLite;
using System;

namespace SosTrackerApp
{
    public class TripLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // 救援任务 ID
        public string TripId { get; set; }

        // 经纬度
        public string GeoLocation { get; set; }

        // 时间戳
        public DateTime CreatedAt { get; set; }
    }
}