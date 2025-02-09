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

        public GoogleCalendarService()
        {
            _calendarService = InitializeService().GetAwaiter().GetResult();
        }

        private async Task<CalendarService> InitializeService()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                var secrets = await GoogleClientSecrets.FromStreamAsync(stream);

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets.Secrets,
                    new[] { CalendarService.Scope.Calendar },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SmartCalender"
            });
        }

        
        public async Task CreateEvent(EventDetails eventDetails)
        {
            var newEvent = new Event()
            {
                Summary = eventDetails.Title,
                Location = eventDetails.Location,
                Description = eventDetails.Description,
                Start = new EventDateTime()
                {
                    DateTime = eventDetails.Start,
                    TimeZone = "Asia/Jerusalem"
                },
                End = new EventDateTime()
                {
                    DateTime = eventDetails.End,
                    TimeZone = "Asia/Jerusalem"
                },

            };

            var calendarId = "primary";
            await _calendarService.Events.Insert(newEvent, calendarId).ExecuteAsync();
        }

    }
}
