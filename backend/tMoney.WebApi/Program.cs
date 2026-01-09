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
        corsPolicyName = "DevelopmentPolicy";
    });
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Remove("Cross-Origin-Opener-Policy");
        context.Response.Headers.Remove("Cross-Origin-Embedder-Policy");
        await next();
    });
}

app.UseCors(corsPolicyName);

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
