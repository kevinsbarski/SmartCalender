using Google.Apis.Calendar.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using SmartCalender.API.Models;
using Google.Apis.Calendar.v3.Data;
using SmartCalender.API.Models.Configuration;
using System.Runtime;

namespace SmartCalender.API.Services.CalenderService
{

    public class GoogleCalendarService : ICalendarService
    {
        private readonly IGoogleApiSettings _googleApiSettings;

        public GoogleCalendarService(IGoogleApiSettings googleApiSettings)
        {
            _googleApiSettings = googleApiSettings;




        

        }


        public async Task<Event> CreateEvent(CalendarEvent calendarEvent)
        {

            UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets()
            {
                ClientId = _googleApiSettings.ClientId,
                ClientSecret = _googleApiSettings.ClientSecret
            },
            new[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents },
            _googleApiSettings.User,
            CancellationToken.None);


            var services = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _googleApiSettings.ApplicationName,
            });



            var newEvent = new Event
            {
                Summary = calendarEvent.Summary,
                Location = calendarEvent.Location,
                Description = calendarEvent.Description,
                Start = new EventDateTime
                {
                    DateTimeDateTimeOffset = calendarEvent.Start,
                    TimeZone = "Asia/Jerusalem"
                },
                End = new EventDateTime
                {
                    DateTimeDateTimeOffset = calendarEvent.End,
                    TimeZone = "Asia/Jerusalem"
                }
            };
            var eventRequest = services.Events.Insert(newEvent, _googleApiSettings.CalendarId);
            var requestCreate = await eventRequest.ExecuteAsync();
            return requestCreate;

            
        }

    }
}
