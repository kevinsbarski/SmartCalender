using Google.Apis.Gmail.v1.Data;
namespace SmartCalender.API.Services.MailSevice;

public interface IGmailService
{
    Task<List<Message>> GetEmailAsync(string query = null);
    Task<string> GetEmailBodyAsync(string messageId);
    Task<IList<Message>> ListAndFetchFullAsync(string query = null);
}

