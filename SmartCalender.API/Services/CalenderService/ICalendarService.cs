using SmartCalender.API.Models;

namespace SmartCalender.API.Services.CalenderService
{
    public interface ICalendarService
    {
        void CreateEvent(EventDetails calendarEvent);
    }
}
