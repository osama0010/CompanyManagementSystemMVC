using Company.DAL.Models;
using Company.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.PL.Helpers
{
    public class EmailSettings : IEmailSettings
    {
        private readonly IOptions<MailSettings> _options;

        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options;
        }
        #region Old Code
        //public static void SendEmail(Email email)
        //{
        //    var client = new SmtpClient("smtp.gmail.com", 587);
        //    client.EnableSsl = true;
        //    client.Credentials = new NetworkCredential("abcd@mail.com", "sacdadasxcasaa");
        //    client.Send("abcd@mail.com", email.To, email.Subject, email.Body);

        //} 
        #endregion
        public void SendMail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Value.Email),
                Subject = email.Subject           
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            var builder = new BodyBuilder();
            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.Password);
            smtp.Send(mail);

            smtp.Disconnect(true);

        }
    }
}
