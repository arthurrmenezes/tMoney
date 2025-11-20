using tMoney.Application;
using tMoney.Infrastructure;

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

app.Run();
