using System;
using PowerTradePosition.API.Models.Dto;

namespace PowerTradePosition.API.Services;

public interface IReportService
{
  void GetReports();

  void GetReport(string id);
}
