using System;

namespace PowerTradePosition.Reporting.Models;

public interface IReportJob
{
  string JobType { get; }
  Guid Id { get; set; }
  DateTime TriggerTime { get; set; }
  int RetryCount { get; set; }
}
