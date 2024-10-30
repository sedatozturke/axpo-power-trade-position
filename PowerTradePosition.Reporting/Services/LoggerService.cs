using System;
using Serilog;

namespace PowerTradePosition.Reporting.Services;

public class LoggerService : ILoggerService
{
    private readonly ILogger _logger;

    public LoggerService(ILogger logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogWarning(string message)
    {
        _logger.Warning(message);
    }

    public void LogError(string message, Exception ex)
    {
        _logger.Error(ex, message);
    }
}
