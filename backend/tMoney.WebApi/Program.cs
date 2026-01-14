using tMoney.Application;
using tMoney.Infrastructure;
using tMoney.WebApi.Middlewares;
using tMoney.WebApi.WorkerServices.TransactionContext;

var builder = WebApplication.CreateBuilder(args);

#region Infrastructure Dependency Injection

builder.Services.ApplyInfrastructureDependencyInjection(builder.Configuration);

#endregion

#region Application Dependency Injection

builder.Services.ApplyApplicationDependencyInjection();

#endregion

#region Worker Services Dependency Injection

builder.Services.AddHostedService<TransactionOverdueJob>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
