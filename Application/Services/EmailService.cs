using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        _smtpServer = emailSettings["SmtpServer"];
        _smtpPort = int.Parse(emailSettings["SmtpPort"]);
        _smtpUser = emailSettings["SmtpUser"];
        _smtpPass = emailSettings["SmtpPass"];
        _fromEmail = emailSettings["FromEmail"];
        _fromName = emailSettings["FromName"];
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(_fromEmail, _fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        using var smtp = new SmtpClient(_smtpServer, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUser, _smtpPass),
            EnableSsl = true
        };

        await Task.Run(() => smtp.Send(mail));
    }
}
