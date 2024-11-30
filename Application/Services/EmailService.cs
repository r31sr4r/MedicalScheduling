using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _sendGridSmtpServer = "smtp.sendgrid.net";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "apikey";
    private readonly string _smtpPass = "";

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mail = new MailMessage
        {
            From = new MailAddress("rafael.reis.a@gmail.com", "FIAP Medical Scheduling App"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        using var smtp = new SmtpClient(_sendGridSmtpServer, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUser, _smtpPass),
            EnableSsl = true
        };

        await Task.Run(() => smtp.Send(mail));
    }
}
