using eVote360_Pro.Core.Application.DTOs.Shared;
using eVote360_Pro.Core.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace eVote360_Pro.Infrastructure.Services
{
    public class EmailService(IOptions<EmailSettings> options) : IEmailService
    {
        private readonly EmailSettings _settings = options.Value;

        public async Task SendEmailAsync(EmailRequest request)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(request.RecipientName, request.ToEmail));
            message.Subject = request.Subject;

            message.Body = new TextPart("html") { Text = request.Body };

            using var client = new SmtpClient();

            // SecureSocketOptions.StartTls para SMTP real (Gmail, Outlook)
            var secureOption = _settings.UseSsl
                ? SecureSocketOptions.StartTls
                : SecureSocketOptions.None;

            await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, secureOption);

            if (_settings.RequiresAuthentication)
                await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPassword);

            await client.SendAsync(message);
            await client.DisconnectAsync(quit: true);
        }
    }
}
