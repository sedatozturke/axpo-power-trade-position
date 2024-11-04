using System;
using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.API.Models;

public class ReportDetail : ReportItem
{
  public List<PowerVolumeByPeriod> PowerVolumes { get; set; }
}
