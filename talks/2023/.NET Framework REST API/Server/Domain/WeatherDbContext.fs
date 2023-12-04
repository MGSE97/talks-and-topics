namespace Weather

open System.Data.Entity

type WeatherDbContext() =
    inherit DbContext()

    member val Dailies : DbSet<WeatherDaily> = base.Set<WeatherDaily>() with get, set