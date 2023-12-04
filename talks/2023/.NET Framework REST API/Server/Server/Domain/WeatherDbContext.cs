using System.Data.Entity;

namespace Server.Domain
{
    public class WeatherDbContext : DbContext
    {
        public DbSet<WeatherDaily> Dailies { get; set; }
    }
}