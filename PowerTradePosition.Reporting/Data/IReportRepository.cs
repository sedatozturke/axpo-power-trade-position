using System;

namespace PowerTradePosition.Reporting.Data;

public interface IReportRepository
{
  void ListReports();

  void GetReport(string id);
}
