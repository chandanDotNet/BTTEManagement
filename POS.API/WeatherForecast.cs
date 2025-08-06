using System;

namespace POS.API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast'
    public class WeatherForecast
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.Date'
        public DateTime Date { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.Date'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.TemperatureC'
        public int TemperatureC { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.TemperatureC'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.TemperatureF'
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.TemperatureF'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.Summary'
        public string Summary { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WeatherForecast.Summary'
    }
}
