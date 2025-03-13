using Google.Apis.Gmail.v1.Data;
using SmartCalender.API.Models;
namespace SmartCalender.API.Services.MailSevice;

public interface IGmailService
{
    Task<List<Message>> GetEmailAsync(string query = null);
    Task<List<EmailDto>> GetEmailListAsync(string query = null);
    Task<EmailDto> GetEmailDtoAsync(string messageId);


}

