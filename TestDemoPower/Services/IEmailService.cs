using TestDemoPower.Model;

namespace TestDemoPower.Services
{
    public interface IEmailService
    {

        void SendEmail(Email request);
        void SendRegistrationDetails(string toEmail, string username, string password);
    }
}
