using Mbiza.NinetyOne.TopScorers.Api.Middleware;
using Mbiza.NinetyOne.TopScorers.Application.Interfaces;
using Mbiza.NinetyOne.TopScorers.Infrastructure;
using Mbiza.NinetyOne.TopScorers.Infrastructure.Repositories;
using Mbiza.NinetyOne.TopScorers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<MbizaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NinetyOneDB"));
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Liso Mbiza - Get Top Scorers Api", Version = "v1" });
});
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: "Mbiza.NinetyOne.Service", serviceVersion: "1.0.0"))
               .AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddConsoleExporter();
    })
    .WithTracing(tracer =>
    {
        tracer.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: "Mbiza.NinetyOne.Service", serviceVersion: "1.0.0"))
              .AddAspNetCoreInstrumentation()
              .AddHttpClientInstrumentation()
              .AddConsoleExporter();
    });

builder.Services.AddTransient<ITopScorersService, TopScorersService>();
builder.Services.AddTransient<ITopScorerRepository, TopScorerRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Liso Mbiza - Get Top Scorers Api v1"));
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health");

// DB Migration
using (IServiceScope scope = app.Services.CreateScope())
{
    MbizaDbContext dbContext = scope.ServiceProvider.GetRequiredService<MbizaDbContext>();
    try
    {
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("? Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"? Migration error: {ex.Message}");
    }
}

app.Run();
