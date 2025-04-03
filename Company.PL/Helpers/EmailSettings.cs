using Company.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("abcd@mail.com", "sacdadasxcasaa");
            client.Send("abcd@mail.com", email.To, email.Subject, email.Body);

        }
    }
}
