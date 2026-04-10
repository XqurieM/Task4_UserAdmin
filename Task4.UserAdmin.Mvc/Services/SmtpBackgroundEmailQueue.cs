using System.Net;
using System.Net.Mail;
using System.Threading.Channels;
using Microsoft.Extensions.Options;
using Task4.UserAdmin.Application.Interfaces;

namespace Task4.UserAdmin.Mvc.Services;

public sealed class SmtpBackgroundEmailQueue : BackgroundService, IEmailQueue
{
    private readonly Channel<EmailMessage> _channel = Channel.CreateUnbounded<EmailMessage>();
    private readonly SmtpOptions _smtpOptions;
    private readonly ILogger<SmtpBackgroundEmailQueue> _logger;

    public SmtpBackgroundEmailQueue(IOptions<SmtpOptions> smtpOptions, ILogger<SmtpBackgroundEmailQueue> logger)
    {
        _smtpOptions = smtpOptions.Value;
        _logger = logger;
    }

    public ValueTask QueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
        => _channel.Writer.WriteAsync(message, cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
                {
                    EnableSsl = _smtpOptions.EnableSsl,
                    Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password)
                };

                using var mail = new MailMessage
                {
                    From = new MailAddress(_smtpOptions.FromEmail, _smtpOptions.FromDisplayName),
                    Subject = message.Subject,
                    Body = message.HtmlBody,
                    IsBodyHtml = true
                };

                mail.To.Add(message.To);
                await client.SendMailAsync(mail, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Asynchronous email delivery failed.");
            }
        }
    }
}
