using Microsoft.AspNetCore.Identity;
using tMoney.Application;
using tMoney.Infrastructure;
using tMoney.Infrastructure.Auth.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Infrastructure Dependency Injection

var databaseConnectionString = builder.Configuration["Database:ConnectionString"];

builder.Services.ApplyInfrastructureDependencyInjection(
    connectionString: databaseConnectionString!);

#endregion

#region Application Dependency Injection

builder.Services.ApplyApplicationDependencyInjection();

#endregion

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IEmailSender<User>, NoOpEmailSender>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();

public class NoOpEmailSender : IEmailSender<User>
{
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        => Task.CompletedTask;

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        => Task.CompletedTask;

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        => Task.CompletedTask;
}