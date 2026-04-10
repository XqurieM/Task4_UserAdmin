using Task4.UserAdmin.Application.Interfaces;

namespace Task4.UserAdmin.WebApi.Services;

public sealed class NullEmailQueue : IEmailQueue
{
    public ValueTask QueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
        => ValueTask.CompletedTask;
}
