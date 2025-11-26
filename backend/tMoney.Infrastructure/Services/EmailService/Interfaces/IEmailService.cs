using tMoney.Infrastructure.Services.EmailService.Inputs;

namespace tMoney.Infrastructure.Services.EmailService.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(SendEmailServiceInput input, CancellationToken cancellationToken);
}
