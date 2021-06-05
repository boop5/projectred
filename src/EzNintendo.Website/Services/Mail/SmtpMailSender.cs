using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EzNintendo.Website.Services.Mail
{
    internal class SmtpMailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("mail.lsc.pw", 587)
            {
                Credentials = new LscPwSmtpCredentials(),
                EnableSsl = true
            };

            var msg = new MailMessage("noreply@lsc.pw", email, subject, htmlMessage) { IsBodyHtml = true };

            return client.SendMailAsync(msg);
        }

        internal class LscPwSmtpCredentials : ICredentialsByHost
        {
            public NetworkCredential GetCredential(string host, int port, string authenticationType)
            {
                return new NetworkCredential("root@lsc.pw", "vT4cVcIaLSLWPq2U3ErPz9BvAd4P5juz");
            }
        }
    }
}