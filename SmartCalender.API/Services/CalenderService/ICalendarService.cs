using SmartCalender.API.Models;

namespace SmartCalender.API.Services.CalenderService
{
    public interface ICalendarService
    {
        Task<CalendarEvent> CreateEvent(CalendarEvent calendarEvent);
    }
}
