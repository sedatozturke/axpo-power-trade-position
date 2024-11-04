using System;
using PowerTradePosition.API.Models;

namespace PowerTradePosition.API.Data;

public interface IReportRepository
{
    List<ReportItem> List();
    
    List<ReportItem> List(string searchQuery);

    ReportDetail? Get(string id);
}
