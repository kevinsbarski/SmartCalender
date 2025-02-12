using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;
using SmartCalender.API.Services.CalenderService;
using SmartCalender.API.Services.ParsingSevice;

namespace SmartCalender.API.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IParsingService _parsingService;
        private readonly ICalendarService _calendarService;

        public EventService(IParsingService parsingService, ICalendarService calendarService)
        {
            _parsingService = parsingService;
            _calendarService = calendarService;
        }

        
        public async Task<EventResponse> ParseEventFromText(string text)
        {
            var parsedEvent = await _parsingService.ParseEventFromTextAsync(text);
            return new EventResponse
            {
                ParsedEvent = parsedEvent.ParsedEvent,
                Message = parsedEvent.Message
            };
        }
  
        public async Task<EventResponse> CreateCalendarEvent(CalendarEvent eventDetails)
        {
            await _calendarService.CreateEvent(eventDetails);
            return new EventResponse
            {
                ParsedEvent = eventDetails,
                Message = "Event was created sucssfuly"
            };

        }
    }
}
