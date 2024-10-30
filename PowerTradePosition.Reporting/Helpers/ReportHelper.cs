using System;

namespace PowerTradePosition.Reporting.Helpers;

public static class ReportHelper
{
  public static string GenerateFileName(string reportType, DateTime reportDate, DateTime triggerDate)
  {
    return $"{reportType}_{reportDate.ToString("yyyyMMdd")}_{triggerDate.ToUniversalTime().ToString("yyyyMMddHHmm")}";
  }

  public static string GenerateFileName(string reportType, DateTime reportDate, DateTime triggerDate, string seperator)
  {
    return $"{reportType}_{reportDate.ToString("yyyyMMdd")}{seperator}{triggerDate.ToUniversalTime().ToString("yyyyMMddHHmm")}";
  }
}
