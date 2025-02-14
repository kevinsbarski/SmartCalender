using Google.Apis.Calendar.v3.Data;
using SmartCalender.API.Models;

namespace SmartCalender.API.Services.EventService
{
    public interface IEventService
    {
        Task<CalendarEvent> ParseEventFromText(string text);
        Task<Event> CreateCalendarEvent(CalendarEvent eventDetails);
    }
}
