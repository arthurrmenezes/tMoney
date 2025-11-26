using tMoney.Application;
using tMoney.Infrastructure;
using tMoney.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Infrastructure Dependency Injection

builder.Services.ApplyInfrastructureDependencyInjection(builder.Configuration);

#endregion

#region Application Dependency Injection

builder.Services.ApplyApplicationDependencyInjection();

#endregion

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
