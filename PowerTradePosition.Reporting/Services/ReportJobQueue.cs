using System;
using System.Collections.Concurrent;
using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.Reporting.Services;

public class ReportJobQueue : IReportJobQueue
{
    private ConcurrentQueue<IReportJob> _jobQueue = new ConcurrentQueue<IReportJob>();

    public ReportJobQueue()
    {
    }

    public IReportJob? Consume()
    {
        _jobQueue.TryDequeue(out var job);
        return job;
    }

    public void Push(IReportJob reportJob)
    {
        _jobQueue.Enqueue(reportJob);
    }

    public int GetQueueLength()
    {
        return _jobQueue.Count;
    }
}
