namespace Weather

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema
open Weather

[<Table("daily", Schema = "weather")>]
type WeatherDaily = 
    { 
        [<Column(Order = 2); Key>] Date: DateTime
        [<Column(Order = 1); Key>] Location: string
        [<Column>] Weather: WeatherType
        [<Column>] Temperature: double 
        [<Column>] Rain: double
    }


