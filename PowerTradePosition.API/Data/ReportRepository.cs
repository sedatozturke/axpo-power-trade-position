using System;
using System.Globalization;
using Microsoft.Extensions.Options;
using PowerTradePosition.API.Models;
using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.API.Data;

public class ReportRepository : IReportRepository
{


    public ReportRepository()
    {

    }
    public ReportDetail? Get(string id)
    {
        try
        {
            string[] files = Directory.GetFiles("../PowerTradePosition.Reporting/output", $"*_{id}.csv", SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                List<PowerVolumeByPeriod> powerVolumes = [];
                var csvLines = File.ReadAllLines(files[0]);
                for (int i = 1; i < csvLines.Length; i++)
                {
                    var powerVolumeColumns = csvLines[i].Split(";");
                    powerVolumes.Add(new PowerVolumeByPeriod
                    {
                        PeriodTime = powerVolumeColumns[0],
                        Volume = double.Parse(powerVolumeColumns[1], CultureInfo.InvariantCulture)
                    });
                }

                var reportMetadata = Path.GetFileNameWithoutExtension(files[0]).Split("_");
                return new ReportDetail
                {
                    Id = reportMetadata[2],
                    Name = Path.GetFileNameWithoutExtension(files[0]),
                    ReportDate = DateTime.ParseExact(reportMetadata[1], "yyyyMMdd", CultureInfo.InvariantCulture),
                    TriggerDateUTC = DateTime.ParseExact(reportMetadata[2], "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                    Type = "Day Ahead Report",
                    PowerVolumes = powerVolumes
                };
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
                                                        ReportDate = DateTime.ParseExact(report[1], "yyyyMMdd", CultureInfo.InvariantCulture),
                                                        TriggerDateUTC = DateTime.ParseExact(report[2], "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
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

    public List<ReportItem> List(string searchQuery)
    {
        try
        {
            string[] files = Directory.GetFiles("../PowerTradePosition.Reporting/output", "*.csv", SearchOption.TopDirectoryOnly);
            var filteredFiles = files.Where(file => Path.GetFileName(file).Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            List<ReportItem> reportItems = filteredFiles.Select(fileNames => Path.GetFileNameWithoutExtension(fileNames))
                                                .Select(file =>
                                                {
                                                    var report = file.Split("_");
                                                    return new ReportItem
                                                    {
                                                        Id = report[2],
                                                        Name = file,
                                                        ReportDate = DateTime.ParseExact(report[1], "yyyyMMdd", CultureInfo.InvariantCulture),
                                                        TriggerDateUTC = DateTime.ParseExact(report[2], "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
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
