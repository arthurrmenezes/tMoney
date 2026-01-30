using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.RateLimiting;
using tMoney.Application;
using tMoney.Infrastructure;
using tMoney.WebApi.Middlewares;
using tMoney.WebApi.WorkerServices.TokenService;
using tMoney.WebApi.WorkerServices.TransactionContext;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration.AddEnvironmentVariables();

#region Infrastructure Dependency Injection

builder.Services.ApplyInfrastructureDependencyInjection(builder.Configuration);

#endregion

#region Application Dependency Injection

builder.Services.ApplyApplicationDependencyInjection();

#endregion

#region Worker Services Dependency Injection

builder.Services.AddHostedService<TransactionOverdueJob>();
builder.Services.AddHostedService<DeleteInvalidRefreshTokensJob>();

#endregion

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

#region Rate Limiting Configuration

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("default_read", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonym";

        return RateLimitPartition.GetTokenBucketLimiter(
            partitionKey: ip,
            factory: _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 60,
                QueueLimit = 10,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                ReplenishmentPeriod = TimeSpan.FromSeconds(1),
                TokensPerPeriod = 2,
                AutoReplenishment = true
            });
    });

    options.AddPolicy("default_write", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonym";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });

    options.AddPolicy("auth", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonym";

        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: ip,
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 4,
                AutoReplenishment = true,
                QueueLimit = 0
            });
    });

    options.AddPolicy("email", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonym";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(10),
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });
});

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

#region CORS Configuration

var corsPolicyName = "ProductionPolicy";
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevelopmentPolicy", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
    corsPolicyName = "DevelopmentPolicy";
}
else
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("ProductionPolicy", policy =>
        {
            policy.WithOrigins("https://tmoney.lovable.app")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

#endregion

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (!app.Environment.IsDevelopment())
    app.UseHsts();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(corsPolicyName);

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
