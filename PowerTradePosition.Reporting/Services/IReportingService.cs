using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.Reporting.Services;

public interface IReportingService: IJobConsumer
{
  Task<DayAheadPosition> GetDayAheadPosition(DateTime date);
  Task CreateDayAheadReport(IReportJob dayAheadReport);
}
