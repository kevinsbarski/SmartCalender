using Google.Apis.Calendar.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using SmartCalender.API.Models;
using Google.Apis.Calendar.v3.Data;

namespace SmartCalender.API.Services.CalenderService
{
    public class GoogleCalendarService : ICalendarService
    {
        private readonly CalendarService _calendarService;
        private readonly IConfiguration _configuration;//testing

        public static async Task<GoogleCalendarService> CreateAsync(IConfiguration configuration) 
        {
            var calendarService = await InitializeServiceAsync(configuration);  
            return new GoogleCalendarService(calendarService, configuration);
        }


        private GoogleCalendarService(CalendarService calendarService, IConfiguration configuration)
        {
            _calendarService = calendarService;
            _configuration = configuration;
        }

        private static async Task<CalendarService> InitializeServiceAsync(IConfiguration configuration)
        {
            using var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read);

            var clientSecrets = new ClientSecrets
            {
                ClientId = configuration["GoogleApiSettings:ClientId"],
                ClientSecret = configuration["GoogleApiSettings:ClientSecret"]
            };

            //   string credPath = "token.json";
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                new[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents },
                "user",
                CancellationToken.None);
           //     new FileDataStore(credPath, true));

            return new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = configuration["GoogleApiSettings:ApplicationName"]
            });
        }

        public async Task<CalendarEvent> CreateEvent(CalendarEvent calendarEvent)
        {
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

            var calendarId = "primary";
            await _calendarService.Events.Insert(newEvent, calendarId).ExecuteAsync();
            return calendarEvent;
        }

    }
}
