using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MedicalScheduling.Application.Interfaces;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["EmailSettings:SmtpServer"];
        _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
        _smtpUser = configuration["EmailSettings:SmtpUser"];
        _smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS") ?? configuration["EmailSettings:SmtpPass"];
        _fromEmail = Environment.GetEnvironmentVariable("FROM_EMAIL") ?? configuration["EmailSettings:FromEmail"];
        _fromName = configuration["EmailSettings:FromName"];
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
