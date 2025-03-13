using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.WebUtilities;
using SmartCalender.API.Models;
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

            listRequest.Fields = "messages(id,threadId)";

            if (!string.IsNullOrEmpty(query))
            {
                listRequest.Q = query;
            }
            var response = await listRequest.ExecuteAsync();
            return response?.Messages.ToList() ?? new List<Message>();
        }
        public async Task<List<EmailDto>> GetEmailListAsync(string query = null)
        {
            var minimalList = await GetEmailAsync(query);

            var emailDtos = new List<EmailDto>();
            foreach (var msg in minimalList)
            {
                var singleDto = await GetEmailDtoAsync(msg.Id);
                emailDtos.Add(singleDto);
            }

            return emailDtos;
        }
        public async Task<EmailDto> GetEmailDtoAsync(string messageId)
        {
            var service = await GetAuthorizedGmailServiceAsync();
            var getRequest = service.Users.Messages.Get("me", messageId);

            getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

            var message = await getRequest.ExecuteAsync();
            
            string fromValue = "";
            string dateValue = "";
            string subjectValue = "";

            if (message.Payload?.Headers != null)
            {
                var fromHeader = message.Payload.Headers
                    .FirstOrDefault(h => h.Name.Equals("From", StringComparison.OrdinalIgnoreCase));
                if (fromHeader != null)
                    fromValue = fromHeader.Value;

                var dateHeader = message.Payload.Headers
                    .FirstOrDefault(h => h.Name.Equals("Date", StringComparison.OrdinalIgnoreCase));
                if (dateHeader != null)
                    dateValue = dateHeader.Value;

                var subjectHeader = message.Payload.Headers
                    .FirstOrDefault(h => h.Name.Equals("Subject", StringComparison.OrdinalIgnoreCase));
                if (subjectHeader != null)
                    subjectValue = subjectHeader.Value;
            }

            string bodyText = "";
            if (message.Payload?.Parts != null)
            {
                foreach (var part in message.Payload.Parts)
                {
                    if (part.MimeType == "text/plain" && part.Body?.Data != null)
                    {
                        bodyText = Base64UrlDecode(part.Body.Data);
                        break;
                    }
                }
            }
            else if (message.Payload?.Body?.Data != null)
            {
                bodyText = Base64UrlDecode(message.Payload.Body.Data);
            }

            var emailDto = new EmailDto
            {
                Id = messageId,
                Subject = subjectValue,
                From = fromValue,
                DateSent = dateValue,
                Body = bodyText
            };

            return emailDto;
        }

        private static string Base64UrlDecode(string input)
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(input);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
