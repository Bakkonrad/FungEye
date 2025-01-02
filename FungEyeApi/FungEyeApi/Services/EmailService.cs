using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using System.Net;
using System.Net.Mail;

namespace FungEyeApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUserName;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private static readonly string argumentMessage = "There are missing parameters in the configuration for EmailService";

        public EmailService(IConfiguration config)
        {
            // Read and validate configuration
            _smtpHost = config["Smtp:Host"] ?? throw new ArgumentNullException(argumentMessage);
            _smtpPort = int.TryParse(config["Smtp:Port"], out var port) ? port : throw new ArgumentNullException(argumentMessage);
            _smtpUserName = config["Smtp:UserName"] ?? throw new ArgumentNullException(argumentMessage);
            _smtpPassword = config["Smtp:Password"] ?? throw new ArgumentNullException(argumentMessage);
            _enableSsl = bool.TryParse(config["Smtp:EnableSsl"], out var enableSsl) ? enableSsl : throw new ArgumentNullException(argumentMessage);
        }

        public async Task<bool> SendEmailAsync(string toEmail, SendEmailOptionsEnum sendEmailOption, string? link = null)
        {
            (string subject, string message) = GetMessage(sendEmailOption, link);

            try
            {
                // Configure SMTP client
                using var smtpClient = new SmtpClient(_smtpHost)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_smtpUserName, _smtpPassword),
                    EnableSsl = _enableSsl,
                };

                // Create e-mail message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUserName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                // Sending message
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw new Exception("Failed to send email.", ex);
            }
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
                result = linkProvided ? htmlContent.Replace("{0}", link) : htmlContent;
                return result;
            }
            else
            {
                throw new Exception("File not found.");
            }
        }
    }

}
