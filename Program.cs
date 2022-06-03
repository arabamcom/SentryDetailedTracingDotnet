using Castle.Windsor.MsDependencyInjection;
using NLog;
using SentryExample.Api1;
using SentryExample.Core.IoC;
using SentryExample.Core.Logging;
using SentryExample.Core.Models;

var config = NLogConfiguration.ConfigureTarget();
var logger = NLog.Web.NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSentry();
    builder.Services.AddHttpClient();

    builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));

    builder.WebHost.UseSentry();

    WindsorRegistrationHelper.AddServices(ContainerManager.WindsorContainer, builder.Services);

    BootStrapper.InitializeContainer();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSentryTracing();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    // NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}