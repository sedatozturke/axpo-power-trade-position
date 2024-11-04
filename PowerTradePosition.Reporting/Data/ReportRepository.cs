using System;
using Microsoft.Extensions.Options;
using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.Reporting.Data;

public class ReportRepository : IReportRepository
{
    private readonly ReportConfig _reportConfig;

    public ReportRepository(IOptions<ReportConfig> reportConfig)
    {
        _reportConfig = reportConfig.Value;
    }
    public void GetReport(string id)
    {
        throw new NotImplementedException();
    }

    public void ListReports()
    {
        try
        {
            string[] files = Directory.GetFiles("../PowerTradePosition.Reporting/output", "*.csv", SearchOption.TopDirectoryOnly);

            Console.WriteLine($"Files in folder '{_reportConfig.OutputDirectory}':");
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
