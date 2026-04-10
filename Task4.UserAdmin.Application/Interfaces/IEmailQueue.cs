namespace Task4.UserAdmin.Application.Interfaces;

public interface IEmailQueue
{
    ValueTask QueueAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
