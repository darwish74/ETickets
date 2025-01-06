
using System.Net.Mail;
using System.Net;

namespace ETickets.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mohammeddarwish66609@gmail.com", "urbz ulys vvne mmnj")
            };
            return client.SendMailAsync(
                new MailMessage(from: "mohammeddarwish66609@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true
                });
        }
    }
}
