using System;
using Axpo;
using Moq;
using PowerTradePosition.Reporting.Exceptions;
using PowerTradePosition.Reporting.Models;
using PowerTradePosition.Reporting.Services;
using PowerTradePosition.UnitTests.Helpers;

namespace PowerTradePosition.UnitTests.Systems.Models;

public class TestDayAheadPosition
{
    [Fact]
    public async Task DayAheadPosition_WithInvalidAmountOfPeriod_ReturnsCorrectedPeriods()
    {
        // Arrange
        var date = DateTime.Now.Date;
        IEnumerable<PowerTrade> powerTrades = PowerServiceHelper.GetPowerTradesWithInvalidAmountOfPeriod(date, 3);


        var dayAheadPositions = new DayAheadPosition(date, powerTrades);

        // Act & Assert
        Assert.Equal(24, dayAheadPositions.GetTimestampedTotalVolumes().Count);
    }

    [Fact]
    public async Task DayAheadPosition_WithValidData_ReturnsTheCorrectSumOfTrades()
    {
        // Arrange
        var date = DateTime.Now.Date;
        IEnumerable<PowerTrade> powerTrades = PowerServiceHelper.GetValidPowerTrades(date, 3);

        // Act
        var dayAheadPositions = new DayAheadPosition(date, powerTrades);
        var result = dayAheadPositions.GetTimestampedTotalVolumes();

        // Assert
        Assert.Equal(powerTrades.Select(i => i.Periods.First().Volume).Sum(), result[0].Volume);
        Assert.Equal(powerTrades.Select(i => i.Periods.Last().Volume).Sum(), result[23].Volume);
    }

}
