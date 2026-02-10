using System.Net;
using System.Net.Mail;

namespace erp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("hariprakash2109@gmail.com");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("hariprakash2109@gmail.com", "kasyprknmaojffgk"),
                    EnableSsl = true
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Error: " + ex.Message);
                throw; // remove this if you don't want crash
            }
        }

    }
}
