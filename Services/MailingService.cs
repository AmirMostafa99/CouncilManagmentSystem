using CouncilsManagmentSystem.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net;


namespace CouncilsManagmentSystem.Services
{
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailsettings;
        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailsettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(string mailto, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailsettings.Email),
                Subject = subject,
            };
            email.To.Add(MailboxAddress.Parse(mailto));

            var builder = new BodyBuilder();

            // Disable SSL/TLS certificate validation
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailsettings.DsiplayName, _mailsettings.Email));
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailsettings.Host, _mailsettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailsettings.Email, _mailsettings.Password);
            await smtp.SendAsync(email);
            
            smtp.Disconnect(true);
        }
    }

}
