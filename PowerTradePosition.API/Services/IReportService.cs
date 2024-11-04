using System;
using PowerTradePosition.API.Models;
using PowerTradePosition.API.Models.Dto;

namespace PowerTradePosition.API.Services;

public interface IReportService
{
  List<ReportItem> GetReports(string? searchQuery);

  ReportDetail GetReport(string id);
}
