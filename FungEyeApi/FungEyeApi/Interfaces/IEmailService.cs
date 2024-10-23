using FungEyeApi.Enums;

namespace FungEyeApi.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, SendEmailOptionsEnum sendEmailOption, string? link = null);
    }
}
