using System.Net.Mail;
using System.Net;
using FungEyeApi.Interfaces;
using FungEyeApi.Enums;
using Newtonsoft.Json.Linq;

namespace FungEyeApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(string toEmail, SendEmailOptionsEnum sendEmailOption, string? link = null)
        {

            (string subject, string message) = GetMessage(sendEmailOption, link);

            var smtpClient = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(_config["Smtp:UserName"], _config["Smtp:Password"]),
                EnableSsl = bool.Parse(_config["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Smtp:UserName"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }

        private static (string, string) GetMessage(SendEmailOptionsEnum sendEmailOption, string? link = null)
        {
            string subject;
            string message;

            switch (sendEmailOption)
            {
                case SendEmailOptionsEnum.ResetPassword:
                    subject = "Resetowanie hasła FungEye";
                    message = LoadHtmlFile("Resources/resetPasswordMessage.html", link);
                    break;
                case SendEmailOptionsEnum.SetAdminPassword:
                    subject = "Ustawianie hasła dla admina FungEye";
                    message = LoadHtmlFile("Resources/setAdminPasswordMessage.html", link);
                    break;
                case SendEmailOptionsEnum.RemindOfExpiredAccount:
                    subject = "Przypomnienie o wygasłym koncie FungEye";
                    message = LoadHtmlFile("Resources/remindOfExpiredAccountMessage.html", link);
                    break;
                default:
                    subject = "Default subject";
                    message = LoadHtmlFile("Resources/ResetPassword.html");
                    break;
            }

            return (subject, message);
        }

        private static string LoadHtmlFile(string relativePath, string? link = null)
        {
            string result;
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            var linkProvided = !String.IsNullOrWhiteSpace(link);

            if (File.Exists(fullPath))
            {
                string htmlContent = File.ReadAllText(fullPath);
                result = linkProvided ? string.Format(htmlContent, link) : htmlContent;
                return result;
            }
            else
            { 
                throw new Exception("File not found"); 
            }
        }
    }

}
