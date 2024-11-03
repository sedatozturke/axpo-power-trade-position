using System;

namespace PowerTradePosition.API.Models;

public class ReportItem
{
  public string Id { get; set; }
  public string Name { get; set; }
  public DateTime ReportDate { get; set; }
  public DateTime TriggerDate { get; set; }
  public string Type { get; set; }
}
