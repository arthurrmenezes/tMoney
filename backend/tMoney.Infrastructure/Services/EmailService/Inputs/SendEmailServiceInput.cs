namespace tMoney.Infrastructure.Services.EmailService.Inputs;

public sealed class SendEmailServiceInput
{
    public SendEmailServiceInputTo[] To { get; }
    public string HtmlContent { get; }
    public string Subject { get; }

    private SendEmailServiceInput(SendEmailServiceInputTo[] to, string htmlContent, string subject)
    {
        To = to;
        HtmlContent = htmlContent;
        Subject = subject;
    }

    public static SendEmailServiceInput Factory(SendEmailServiceInputTo[] to, string htmlContent, string subject)
        => new(to, htmlContent, subject);
}

public sealed class SendEmailServiceInputTo
{
    public string Name { get; }
    public string Email { get; }

    public SendEmailServiceInputTo(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
