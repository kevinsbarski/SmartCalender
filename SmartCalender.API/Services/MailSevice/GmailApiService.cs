using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.WebUtilities;
using SmartCalender.API.Models.Configuration;
using System.Text;

namespace SmartCalender.API.Services.MailSevice
{
    public class GmailApiService : IGmailService
    {
        private readonly IGoogleApiSettings _googleApiSettings;
        
        public GmailApiService(IGoogleApiSettings googleApiSettings)
        {
            _googleApiSettings = googleApiSettings;
        }
        private async Task<GmailService> GetAuthorizedGmailServiceAsync()
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = _googleApiSettings.ClientId,
                    ClientSecret = _googleApiSettings.ClientSecret
                },
                _googleApiSettings.Scope,
                _googleApiSettings.User,
                CancellationToken.None);

            var gmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _googleApiSettings.ApplicationName
            });
            return gmailService;

        }
        public async Task<List<Message>> GetEmailAsync(string query = null)
        {
            var service = await GetAuthorizedGmailServiceAsync();
            var listRequest = service.Users.Messages.List("me");

            listRequest.Fields = "messages(id,threadId,snippet)";

            if (!string.IsNullOrEmpty(query))
            {
                listRequest.Q = query;
            }
            var response = await listRequest.ExecuteAsync();
            return response?.Messages.ToList() ?? new List<Message>();
        }

        public async Task<string> GetEmailBodyAsync(string messageId)
        {
            var service = await GetAuthorizedGmailServiceAsync();
            var getRequest = service.Users.Messages.Get("me", messageId);
            getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

            var message = await getRequest.ExecuteAsync();

            string emailBody = "";
            if(message.Payload?.Parts != null)
            {
                foreach (var part in message.Payload.Parts)
                {
                    if(part.MimeType == "text/plain" && part.Body?.Data != null)
                    {
                        emailBody = Base64UrlDecode(part.Body.Data);
                        break;

                    }
                }
            }
            else if (message.Payload?.Body?.Data != null)
            {
                emailBody = Base64UrlDecode(message.Payload.Body.Data);
            }
            return emailBody;

        }
        public async Task<IList<Message>> ListAndFetchFullAsync(string query = null)
        {
            // 1) List messages to get IDs
            var service = await GetAuthorizedGmailServiceAsync();
            var listRequest = service.Users.Messages.List("me");
            if (!string.IsNullOrEmpty(query)) listRequest.Q = query;
            var response = await listRequest.ExecuteAsync();
            var minimalList = response?.Messages ?? new List<Message>();

            // 2) For each message ID, do a full GET
            var fullMessages = new List<Message>();
            foreach (var msg in minimalList)
            {
                var getRequest = service.Users.Messages.Get("me", msg.Id);
                getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
                var fullMsg = await getRequest.ExecuteAsync();

                fullMessages.Add(fullMsg);
            }
            return fullMessages;
        }

        private static string Base64UrlDecode(string input)
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(input);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
