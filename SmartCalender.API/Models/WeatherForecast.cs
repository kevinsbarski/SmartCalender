using System;

namespace SmartCalender.API.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; } // Using DateTime from plan example
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public int TemperatureMinC { get; set; }
        public int TemperatureMaxC { get; set; }
    }
        public int MinimumTemperatureC { get; set; }
        public int MaximumTemperatureC { get; set; }
        public int MinimumTemperatureF => 32 + (int)(MinimumTemperatureC / 0.5556);
        public int MaximumTemperatureF => 32 + (int)(MaximumTemperatureC / 0.5556);
}