using SQLite;
using System;

namespace Health.Models
     {
    public class HealthData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Steps { get; set; }
        public double Calories { get; set; }
        public double Distance { get; set; }
    }
     }
     