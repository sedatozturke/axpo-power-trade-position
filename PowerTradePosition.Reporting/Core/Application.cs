using System;
using Microsoft.Extensions.Options;
using PowerTradePosition.Reporting.Extensions;
using PowerTradePosition.Reporting.Models;
using PowerTradePosition.Reporting.Services;

namespace PowerTradePosition.Reporting.Core;

public class Application : IApplication
{
    private readonly IReportingService _reportingService;
    private readonly IReportJobQueue _reportJobQueue;
    private readonly ReportConfig _reportConfig;
    private readonly ILoggerService _loggerService;
    public Application(IReportingService reportingService, IReportJobQueue reportJobQueue, IOptions<ReportConfig> reportConfig, ILoggerService loggerService)
    {
        _reportingService = reportingService;
        _reportJobQueue = reportJobQueue;
        _reportConfig = reportConfig.Value;
        _loggerService = loggerService;
    }

    public void Start()
    {
        var interval = _reportConfig.Interval > 0 ? _reportConfig.Interval : 5;

        PushJobToReportQueue(null);
        var _timer = new Timer(PushJobToReportQueue, null, TimeSpan.FromMinutes(interval), TimeSpan.FromMinutes(interval));

        _loggerService.LogInformation($"Subscribing reporting service to its job queue");
        _reportingService.SubscribeToJobQueue(TimeSpan.FromSeconds(15));
    }

    public void Stop()
    {
        _reportingService.UnsubscribeFromJobQueue();
    }

    private void PushJobToReportQueue(object? state)
    {
        var id = Guid.NewGuid();
        var triggerTime = DateTime.Now;
        _loggerService.LogInformation($"Creating a Day Ahead Report Job. Job Id: {id} Request Time(UTC): {triggerTime.ToUTCString()}");
        _reportJobQueue.Push(new DayAheadReportJob
        {
            Id = id,
            TriggerTime = triggerTime,
            RetryCount = 0
        });
    }
}
