using System;
using Axpo;
using PowerTradePosition.Reporting.Models;
using PowerTradePosition.Reporting.Exceptions;
using PowerTradePosition.Reporting.Extensions;
using PowerTradePosition.Reporting.Helpers;

namespace PowerTradePosition.Reporting.Services;

public class ReportingService : IReportingService
{
    private readonly IPowerService _powerService;

    private readonly IRecorderService _recorderService;

    private readonly IReportJobQueue _reportJobQueue;

    private readonly ILoggerService _loggerService;
    
    private bool subscribed = false;

    public ReportingService(IPowerService powerService, IRecorderService recorderService, IReportJobQueue reportJobQueue, ILoggerService loggerService)
    {
        _powerService = powerService;
        _recorderService = recorderService;
        _reportJobQueue = reportJobQueue;
        _loggerService = loggerService;
    }
    public async Task<DayAheadPosition> GetDayAheadPosition(DateTime date)
    {
        var result = await _powerService.GetTradesAsync(date);
        if (!result.Any())
        {
            throw new ServiceResultEmptyException();
        }
        
        _loggerService.LogInformation($"Got day ahead trades from service for Date: {date.ToString("yyyy-MM-dd")}");
        var position = new DayAheadPosition(date, result);
        return position;
    }

    public virtual async Task CreateDayAheadReport(IReportJob dayAheadReportJob)
    {
        try
        {
            var triggerDate = dayAheadReportJob.TriggerTime;
            var reportDate = triggerDate.AddDays(1).Date;
            _loggerService.LogInformation(
                $"Processing {dayAheadReportJob.JobType}. Job Id: {dayAheadReportJob.Id} Extraction Time(UTC): {dayAheadReportJob.TriggerTime.ToUTCString()}"
                );
            var dayAheadPosition = await GetDayAheadPosition(reportDate);

            _recorderService.WriteAsCSV(
                ReportHelper.GenerateFileName("PowerPosition", reportDate, triggerDate),
                dayAheadPosition.GetTimestampedTotalVolumes().AsCSVRows(";"),
                "DateTime;Volume"
                );
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"Exception while creating a report: Type: {dayAheadReportJob.JobType} Id:{dayAheadReportJob.Id}", ex);
            if (dayAheadReportJob.RetryCount < 3)
            {
                dayAheadReportJob.RetryCount += 1;
                _reportJobQueue.Push(dayAheadReportJob);
                _loggerService.LogInformation($"Pushed Job({dayAheadReportJob.Id}) to queue for retry");
            }
            else
            {
                _loggerService.LogInformation($"Maximum retry count reached for Job({dayAheadReportJob.Id}), will not retry.");
            }
        }
    }

    public async Task SubscribeToJobQueue(TimeSpan interval)
    {
        subscribed = true;
        while (subscribed)
        {
            _loggerService.LogInformation("Checking Report Job Queue...");
            var job = _reportJobQueue.Consume();
            if (job is not null)
            {
                _loggerService.LogInformation($"Found a {job.JobType} job in queue.");
                switch (job.JobType)
                {
                    case "DayAheadReport":
                        CreateDayAheadReport(job);
                        break;
                    default:
                        _loggerService.LogWarning($"Couldn't find a processor for Job({job.JobType}). Job Type: {job.JobType}");
                        job.RetryCount += 1;
                        _reportJobQueue.Push(job);
                        break;
                }
            }
            else
            {
                _loggerService.LogInformation("Queue is empty.");
            }
            await Task.Delay(interval);
        }
    }

    public void UnsubscribeFromJobQueue()
    {
        subscribed = false;
    }
}
