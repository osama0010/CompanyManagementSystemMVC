using Company.DAL.Models;

namespace Company.PL.Helpers
{
    public interface IEmailSettings
    {
        public void SendMail(Email mail);
    }
}
