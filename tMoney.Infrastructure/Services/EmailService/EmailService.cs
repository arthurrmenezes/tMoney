using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using tMoney.Infrastructure.Services.EmailService.Inputs;
using tMoney.Infrastructure.Services.EmailService.Interfaces;

namespace tMoney.Infrastructure.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public EmailService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(SendEmailServiceInput input, CancellationToken cancellationToken)
    {
        var toList = input.To.Select(t => new SendEmailServiceInputTo(
            name: t.Name,
            email: t.Email)).ToArray();

        var payload = SendEmailServiceInput.Factory(
            sender: new SendEmailServiceInputSender(
                name: _configuration["SMTP:SenderName"]!,
                email: _configuration["SMTP:SenderEmail"]!),
            to: toList,
            content: input.Content,
            subject: input.Subject);

        var request = new HttpRequestMessage(HttpMethod.Post, _configuration["SMTP:BaseUrl"]!);

        request.Headers.Add(
            name: "api-key",
            value: _configuration["SMTP:ApiKey"]!);
        request.Headers.Add(
            name: "accept",
            value: "application/json");

        request.Content = JsonContent.Create(payload);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Erro ao enviar email: {responseString}");
    }
}
