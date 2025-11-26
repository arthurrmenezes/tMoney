namespace tMoney.Infrastructure.Services.EmailService.Payloads;

public sealed class SendEmailBrevoPayload
{
    public SendEmailBrevoPayloadSender Sender { get; init; }
    public SendEmailBrevoPayloadTo[] To { get; init; }
    public string HtmlContent { get; init; }
    public string Subject { get; init; }

    private SendEmailBrevoPayload(SendEmailBrevoPayloadSender sender, SendEmailBrevoPayloadTo[] to, string htmlContent, string subject)
    {
        Sender = sender;
        To = to;
        HtmlContent = htmlContent;
        Subject = subject;
    }

    public static SendEmailBrevoPayload Factory(SendEmailBrevoPayloadSender sender, SendEmailBrevoPayloadTo[] to, string htmlContent, string subject)
        => new(sender, to, htmlContent, subject);
}

public sealed class SendEmailBrevoPayloadSender
{
    public string Name { get; }
    public string Email { get; }

    public SendEmailBrevoPayloadSender(string name, string email)
    {
        Name = name;
        Email = email;
    }

}

public sealed class SendEmailBrevoPayloadTo
{
    public string Name { get; }
    public string Email { get; }

    public SendEmailBrevoPayloadTo(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
