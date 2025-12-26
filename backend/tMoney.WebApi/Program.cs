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

#region CORS Configuration

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });

    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins("https://tmoney.onrender.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ProductionPolicy");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
