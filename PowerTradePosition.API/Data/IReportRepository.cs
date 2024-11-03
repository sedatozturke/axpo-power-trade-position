using System;
using PowerTradePosition.API.Models;

namespace PowerTradePosition.API.Data;

public interface IReportRepository
{
    List<ReportItem> List();

    ReportDetail Get(string id);
}
