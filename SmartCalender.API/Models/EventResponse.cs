namespace SmartCalender.API.Models;

public class EventResponse
{
    public CalendarEvent ParsedEvent { get; set; }
    public string Message { get; set; }
}
