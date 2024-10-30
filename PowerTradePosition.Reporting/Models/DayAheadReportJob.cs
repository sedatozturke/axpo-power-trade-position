using System;

namespace PowerTradePosition.Reporting.Models;

public class DayAheadReportJob : IReportJob
{
    public string JobType { get => "DayAheadReport";}
    public Guid Id { get; set; }
    public DateTime TriggerTime { get; set; }
    public int RetryCount { get; set; }
}
