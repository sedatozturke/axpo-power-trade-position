using System;
using System.Globalization;
using Microsoft.Extensions.Options;
using PowerTradePosition.API.Models;

namespace PowerTradePosition.API.Data;

public class ReportRepository : IReportRepository
{


    public ReportRepository()
    {

    }
    public ReportDetail Get(string id)
    {
        try
        {
            string[] files = Directory.GetFiles("../PowerTradePosition.Reporting/output", $"*_{id}.csv", SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                var reportMetadata = Path.GetFileNameWithoutExtension(files[0]).Split("_");
                ReportItem reportItem = new ReportItem
                {
                    Id = reportMetadata[2],
                    Name = Path.GetFileNameWithoutExtension(files[0]),
                    ReportDate = DateTime.ParseExact(reportMetadata[1], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
                    TriggerDate = DateTime.ParseExact(reportMetadata[2], "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
                    Type = "Day Ahead Report"
                };

                var csvLines = File.ReadAllLines(files[0]);
                for (int i = 1; i < csvLines.Length; i++)
                {
                    Console.Write(csvLines[i]);
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public List<ReportItem> List()
    {
        try
        {
            string[] files = Directory.GetFiles("../PowerTradePosition.Reporting/output", "*.csv", SearchOption.TopDirectoryOnly);

            List<ReportItem> reportItems = files.Select(fileNames => Path.GetFileNameWithoutExtension(fileNames))
                                                .Select(file =>
                                                {
                                                    var report = file.Split("_");
                                                    return new ReportItem
                                                    {
                                                        Id = report[2],
                                                        Name = file,
                                                        ReportDate = DateTime.ParseExact(report[1], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
                                                        TriggerDate = DateTime.ParseExact(report[2], "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
                                                        Type = "Day Ahead Report"
                                                    };
                                                }).ToList();
            return reportItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return [];
        }
    }
}
