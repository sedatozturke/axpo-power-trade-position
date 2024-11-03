using System;
using PowerTradePosition.API.Models.Dto;
using PowerTradePosition.API.Data;

namespace PowerTradePosition.API.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }
    public void GetReport(string id)
    {
        _reportRepository.Get(id);
    }

    public void GetReports()
    {
        _reportRepository.List();
    }
}
