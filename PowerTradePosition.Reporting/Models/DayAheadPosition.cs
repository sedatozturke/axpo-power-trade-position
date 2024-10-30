using System;
using Axpo;

namespace PowerTradePosition.Reporting.Models;

public class DayAheadPosition
{
  private double[] periodValuesArray = new double[24];

  private readonly DateTime _reportDate;

  private readonly IEnumerable<PowerTrade> _powerTrades;

  private List<PowerVolume> timestampedPositionList = [];

  public DayAheadPosition(DateTime reportDate, IEnumerable<PowerTrade> powerTrades)
  {
    _reportDate = reportDate;
    _powerTrades = powerTrades;
    timestampedPositionList = CreateTotalVolumeWithPeriods();
  }

  private void Aggregate(int key, double value)
  {
    if (key <= periodValuesArray.Length && key > 0)
    {
      var currentValue = periodValuesArray[key-1];
      periodValuesArray[key-1] = currentValue + value;
    }
  }

  private List<PowerVolume> GetTimestampedPositions()
  {
    var list = new List<PowerVolume>();
    var periodDateTime = _reportDate.ToUniversalTime();
    for (var i = 0; i < periodValuesArray.Length; i++)
    {
      var itemDateTime = periodDateTime.AddHours(i);
      list.Add(new PowerVolume
      {
        UTCTime = itemDateTime,
        Volume = periodValuesArray[i]
      });
    }
    return list;
  }

  private List<PowerVolume> CreateTotalVolumeWithPeriods() {
        foreach (var ptItem in _powerTrades)
        {
            foreach (var periodItem in ptItem.Periods)
            {
                Aggregate(periodItem.Period, periodItem.Volume);
            }
        }
        return GetTimestampedPositions();
    }

  public DateTime GetPositionTime()
  {
    return _reportDate;
  }

  public IEnumerable<PowerTrade> GetPowerTrades()
  {
    return _powerTrades;
  }
  public List<PowerVolume> GetTimestampedTotalVolumes()
  {
    return timestampedPositionList;
  }
  
}
