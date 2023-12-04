using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain
{
    [Table("daily", Schema = "weather")]
    public class WeatherDaily
    {
        [Column(Order = 2), Key]
        public DateTime Date { get; set; }
        
        [Column(Order = 1), Key]
        public string Location { get; set; }

        [Column]
        public WeatherType Type { get; set; }

        [Column]
        public double Temperature { get; set; }

        [Column]
        public double Rain { get; set; }
    }
}