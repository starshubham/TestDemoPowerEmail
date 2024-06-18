using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using TestDemoPower.Model;
using MailKit.Net.Smtp;

namespace TestDemoPower.Services
{
    public class EmailService : IEmailService
    {
        
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
         
        public void SendEmail(Email email)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            emailMessage.To.Add(MailboxAddress.Parse(email.To));
            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = email.Body };

            var emailHost = _config.GetSection("EmailHost").Value;
            var emailUsername = _config.GetSection("EmailUsername").Value;
            var emailPassword = _config.GetSection("EmailPassword").Value;

            if (string.IsNullOrEmpty(emailHost) || string.IsNullOrEmpty(emailUsername) || string.IsNullOrEmpty(emailPassword))
            { 
                throw new ApplicationException("Email configuration is missing or invalid.");
            }

            using var smtp = new SmtpClient();
                  
            smtp.Connect(emailHost, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailUsername, emailPassword);
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }

        public void SendRegistrationDetails(string toEmail, string username, string password)
        {
            var email = new Email
            {
                To = toEmail,
                Subject = "Registration Details",
                Body = $"Your username: {username}\nYour password: {password}"
            };

            SendEmail(email);
        }
    }
}
