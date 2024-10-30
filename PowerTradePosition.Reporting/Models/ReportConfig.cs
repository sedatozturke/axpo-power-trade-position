using System;

namespace PowerTradePosition.Reporting.Models;

public class ReportConfig
{
  public string? OutputDirectory { get; set; } = ".";

  public int Interval { get; set; } = 5;
}
