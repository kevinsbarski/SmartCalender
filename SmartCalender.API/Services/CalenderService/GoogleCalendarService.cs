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
            _calendarService = InitializeService();
        }

        private CalendarService InitializeService()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { CalendarService.Scope.Calendar },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Event Scheduler"
            });
        }

        
        public void CreateEvent(EventDetails eventDetails)
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
            _calendarService.Events.Insert(newEvent, calendarId).Execute();
        }

    }
}
