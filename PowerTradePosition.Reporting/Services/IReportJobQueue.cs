using System;
using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.Reporting.Services;

public interface IReportJobQueue
{
  void Push(IReportJob reportJob);

  IReportJob? Consume();

  int GetQueueLength();

}
