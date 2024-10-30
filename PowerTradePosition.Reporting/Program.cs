using System;
using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerTradePosition.Reporting.Core;
using PowerTradePosition.Reporting.Models;
using PowerTradePosition.Reporting.Services;
using Serilog;

namespace PowerTradePosition;

class Program
{
    static void Main(string[] args)
    {
        var configuration = BuildConfiguration(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            
        var serviceProvider = ConfigureServices(configuration);

        var app = serviceProvider.GetService<IApplication>();

        if (app != null)
        {
            try
            {   
                Log.Information("Application is running... Press any key to quit the application.");
                app.Start();
                Console.Read();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                app.Stop();
                Log.CloseAndFlush();
            }
        }

    }

    private static ServiceProvider ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.Configure<ReportConfig>(configuration.GetSection("ReportConfig"));
        services.AddSingleton(Log.Logger);
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<IReportJobQueue, ReportJobQueue>();
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IRecorderService, RecorderService>();
        services.AddSingleton<IReportingService, ReportingService>();
        services.AddSingleton<IApplication, Application>();

        return services.BuildServiceProvider();
    }

    private static IConfiguration BuildConfiguration(string[] args)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddCommandLine(args)
            .Build();
    }
}
