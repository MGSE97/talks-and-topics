namespace Server.Migrations
{
    using Server.Domain;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<WeatherDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WeatherDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            // Lets create random weather for next 14 days
            var daily = new WeatherDailyRepository(context).Table;
            var rand = new Random();
            var date = DateTime.UtcNow.Date;

            double RandomTemp() => rand.NextDouble()*40;
            double RandomRain() => rand.NextDouble() > 0.9 ? rand.NextDouble()*10 : 0;

            var locations = new[] {
                "Český Těšín",
                "Třinec",
                "Jablunkov",
                "Havířov",
                "Ostrava",
                "Opava",
                "Orlová",
                "Brno",
                "Praha"
            };

            foreach (var location in locations) {
                for (int i = 0; i < 14; i++) {
                    var (temperature, rain) = (RandomTemp(), RandomRain());
                    
                    var type = WeatherType.Sunny;
                    if (rain > 5) type = WeatherType.Stormy;
                    else if (rain > 0.5) type = WeatherType.Rainy;
                    else if (temperature < 20) type = WeatherType.Windy;
                    else if (temperature < 10) type = WeatherType.Cloudy;

                    daily.AddOrUpdate(
                        new WeatherDaily
                        {
                            Date = date.AddDays(i),
                            Location = location,
                            Temperature = temperature,
                            Rain = rain,
                            Type = type,
                        }
                    );
                }
            }
        }
    }
}
