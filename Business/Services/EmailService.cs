using Business.Configurations;
using Business.Context;
using Business.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

public class EmailService
{
    private readonly AppDbContext _context;
    private readonly EmailSettings _emailSettings;

    public EmailService(AppDbContext context, IOptions<EmailSettings> options)
    {
        _context = context;
        _emailSettings = options.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body, int? debtId = null)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        var log = new SentEmail
        {
            ToEmail = to,
            Subject = subject,
            Body = body,
            SentAt = DateTime.Now,
            DebtRecordId = debtId
        };
        _context.SentEmails.Add(log);
        await _context.SaveChangesAsync();
    }
}
