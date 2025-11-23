namespace tMoney.Infrastructure.Services.EmailService.Inputs;


public sealed class SendEmailServiceInput
{
    public SendEmailServiceInputSender Sender { get; }
    public SendEmailServiceInputTo[] To { get; }
    public string Content { get; }
    public string Subject { get; }

    private SendEmailServiceInput(SendEmailServiceInputSender sender, SendEmailServiceInputTo[] to, string content, string subject)
    {
        Sender = sender;
        To = to;
        Content = content;
        Subject = subject;
    }

    public static SendEmailServiceInput Factory(SendEmailServiceInputSender sender, SendEmailServiceInputTo[] to, string content, string subject)
        => new(sender, to, content, subject);
}

public sealed class SendEmailServiceInputSender
{
    public string Name { get; }
    public string Email { get; }

    public SendEmailServiceInputSender(string name, string email)
    {
        Name = name;
        Email = email;
    }
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
