using Google.Apis.Calendar.v3.Data;
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

        public async Task<CalendarEvent> ParseEventFromText(string text)
        {
            return await _parsingService.ParseEventFromTextAsync(text);
       
        }

        public async Task<Event> CreateCalendarEvent(CalendarEvent eventDetails)
        {
            return(await _calendarService.CreateEvent(eventDetails));

        }


    }
}
