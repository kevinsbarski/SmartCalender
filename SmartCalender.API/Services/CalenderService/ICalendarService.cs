using SmartCalender.API.Models;

namespace SmartCalender.API.Services.CalenderService
{
    public interface ICalendarService
    {
        Task CreateEvent(EventDetails calendarEvent);
    }
}
