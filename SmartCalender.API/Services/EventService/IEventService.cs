using SmartCalender.API.Models;

namespace SmartCalender.API.Services.EventService
{
    public interface IEventService
    {
        Task<EventResponse> ParseEventFromText(string text);
        Task<EventResponse> CreateCalendarEvent(EventDetails eventDetails);
    }
}
