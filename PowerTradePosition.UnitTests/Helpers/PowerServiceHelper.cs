using System;
using Axpo;

namespace PowerTradePosition.UnitTests.Helpers;

public static class PowerServiceHelper
{
  public static IEnumerable<PowerTrade> GetValidPowerTrades(DateTime dateTime, int amount)
  {
    var powerTrades = new List<PowerTrade>();
    for (int i = 0; i< amount; i++)
    {
      powerTrades.Add(PowerTrade.Create(dateTime, 24));
    }

    return powerTrades;
  }

  public static IEnumerable<PowerTrade> GetPowerTradesWithInvalidAmountOfPeriod(DateTime dateTime, int amount)
  {
    var powerTrades = new List<PowerTrade>();
    for (int i = 0; i < amount; i++)
    {
      powerTrades.Add(PowerTrade.Create(dateTime, 25));
    }

    return powerTrades;
  }
}
