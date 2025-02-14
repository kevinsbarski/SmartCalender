using Google.Apis.Calendar.v3.Data;
using SmartCalender.API.Models;

namespace SmartCalender.API.Services.CalenderService
{
    public interface ICalendarService
    {
        Task<Event> CreateEvent(CalendarEvent calendarEvent);
    }
}
