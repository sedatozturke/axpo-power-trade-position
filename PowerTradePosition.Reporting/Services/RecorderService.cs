using System.Text;
using Microsoft.Extensions.Options;
using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.Reporting.Services;

public class RecorderService : IRecorderService
{
    private readonly ReportConfig _reportConfig;
    private readonly ILoggerService _loggerService;
    public RecorderService(IOptions<ReportConfig> reportConfig, ILoggerService loggerService)
    {
        _reportConfig = reportConfig.Value;
        _loggerService = loggerService;
    }
    public void WriteAsCSV(string fileName, List<string> data) {
        var outputDirectory = CheckAndCreateFolder(_reportConfig.OutputDirectory);
        var filePath = $"{outputDirectory}/{fileName}.csv";
        using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            foreach (var line in data)
            {
                streamWriter.WriteLine(line);
            }
        }
        _loggerService.LogInformation($"Written CSV File: {filePath}");
    }

    public void WriteAsCSV(string fileName, List<string> data, string headerRow) {
        var outputDirectory = CheckAndCreateFolder(_reportConfig.OutputDirectory);
        var filePath = $"{outputDirectory}/{fileName}.csv";
        using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            streamWriter.WriteLine(headerRow);
            foreach (var line in data)
            {
                streamWriter.WriteLine(line);
            }
        }
        _loggerService.LogInformation($"Written CSV File: {filePath}");
    }

    private string CheckAndCreateFolder(string? outputDirectory)
    {
        if (string.IsNullOrWhiteSpace(outputDirectory) || outputDirectory.Equals("."))
        {
            return ".";
        }
        if (!Directory.Exists(outputDirectory))
        {
            _loggerService.LogInformation($"Directory not exists. Creating {outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            return outputDirectory;
        }
        return outputDirectory;
    }
}
