using CCDbApi.ViewModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CCDbApi.Service
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<MailMessage> SendEmailAsync(EmailViewModel model)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password);
                    smtpClient.EnableSsl = _smtpSettings.EnableSsl;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(_smtpSettings.User, "Rex Systems");
                        mailMessage.To.Add(new MailAddress("mdalamin18053@gmail.com"));
                        mailMessage.Subject = string.IsNullOrWhiteSpace(model.Subject)
                                              ? "Rex Systems Notification"
                                              : model.Subject;

                        mailMessage.Body =model.Content;    
                        mailMessage.IsBodyHtml = true;

                        await smtpClient.SendMailAsync(mailMessage);
                        return mailMessage;
                    }
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.StatusCode} - {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return null;
            }
        }
    
    }

}
