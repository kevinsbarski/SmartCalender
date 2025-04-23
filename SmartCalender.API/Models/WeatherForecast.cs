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
}