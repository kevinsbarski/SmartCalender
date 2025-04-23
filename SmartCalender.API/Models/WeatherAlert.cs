using System;

namespace SmartCalender.API.Models
{
    public class WeatherAlert
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Severity { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string AreaDescription { get; set; } = string.Empty;
        public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}