using System;
using PowerTradePosition.API.Models.Dto;
using PowerTradePosition.API.Data;
using PowerTradePosition.API.Models;

namespace PowerTradePosition.API.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }
    public ReportDetail? GetReport(string id)
    {
        return _reportRepository.Get(id);
    }

    public List<ReportItem> GetReports(string searchQuery)
    {
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            return _reportRepository.List(searchQuery);
        }
        return _reportRepository.List();
    }
}
