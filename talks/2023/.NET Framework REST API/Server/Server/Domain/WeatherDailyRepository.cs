using DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Domain
{
    public class WeatherDailyRepository : Repository<WeatherDaily>
    {
        private readonly WeatherDbContext _weatherContext;

        public WeatherDailyRepository(WeatherDbContext context) : base(context)
        {
            _weatherContext = context;
        }

        public Task<List<string>> GetLocations(string query) 
            => (
                from daily in Table
                where daily.Location.Contains(query)
                group daily by daily.Location into g
                orderby g.Key
                select g.Key
            ).ToListAsync();

        public Task<List<WeatherDaily>> Search7Days(DateTime date, string location)
            => (
                from daily in Table
                where
                        daily.Location == location
                    && daily.Date >= date
                    && daily.Date <= SqlFunctions.DateAdd("day", 7, date)
                select daily
            ).ToListAsync();

        public async Task<List<WeatherDaily>> Country7Days(DateTime date) 
            => (
                    await(
                        from daily in Table
                        where
                                daily.Date >= date
                            && daily.Date <= SqlFunctions.DateAdd("day", 7, date)
                        group daily by SqlFunctions.DatePart("dayofyear", daily.Date) into g
                        orderby g.Key
                        select new
                        {
                            Location = "Country",
                            Date = g.Min(item => item.Date),
                            Type = (int)g.Average(item => (int)item.Type),
                            Rain = g.Average(item => item.Rain),
                            Temperature = g.Average(item => item.Temperature),
                        }
                    ).ToListAsync()
                ).Select(r => new WeatherDaily
                {
                    Location = r.Location,
                    Date = r.Date,
                    Type = (WeatherType)r.Type,
                    Rain = r.Rain,
                    Temperature = r.Temperature,
                }).ToList();
    }
}