using SQLite;
using System;

namespace SosTrackerApp
{
    public class TripLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string TripId { get; set; }     // The user input ID
        public string GeoLocation { get; set; } // "Lat, Long"
        public DateTime CreatedAt { get; set; } // Timestamp
    }
}