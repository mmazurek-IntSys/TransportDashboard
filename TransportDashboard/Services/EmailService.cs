using System.Net;
using System.Net.Mail;

namespace TransportDashboard.Services;

public class EmailService
{
    public async Task SendAsync(string subject, string body)
    {
        using var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(
                "your@gmail.com",
                "app_password"
            ),
            EnableSsl = true
        };

        using var message = new MailMessage
        {
            From = new MailAddress("your@gmail.com"),
            Subject = subject,
            Body = body
        };

        message.To.Add("receiver@gmail.com");

        await client.SendMailAsync(message);
    }
}