namespace SmartCalender.API.Models.Configuration
{
    public interface IGoogleApiSettings
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string[] Scope { get; set; }
        string ApplicationName { get; set; }
        string User { get; set; }
        string CalendarId { get; }
    }
}
