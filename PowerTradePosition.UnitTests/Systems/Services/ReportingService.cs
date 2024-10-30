using System;
using Axpo;
using Moq;
using PowerTradePosition.Reporting.Exceptions;
using PowerTradePosition.Reporting.Models;
using PowerTradePosition.Reporting.Services;
using PowerTradePosition.UnitTests.Helpers;

namespace PowerTradePosition.UnitTests.Systems.Services;

public class TestReportingService
{
    private readonly Mock<IPowerService> _powerServiceMock;
    private readonly Mock<ILoggerService> _loggerServiceMock;
    private readonly Mock<IRecorderService> _recorderServiceMock;
    private readonly Mock<IReportJobQueue> _reportJobQueueMock;
    private readonly IReportingService _reportingService;
    private readonly Mock<ReportingService> _reportingServiceMock;

    public TestReportingService()
    {
        _powerServiceMock = new Mock<IPowerService>();
        _loggerServiceMock = new Mock<ILoggerService>();
        _recorderServiceMock = new Mock<IRecorderService>();
        _reportJobQueueMock = new Mock<IReportJobQueue>();
        _reportingServiceMock = new Mock<ReportingService>(_powerServiceMock.Object, _recorderServiceMock.Object, _reportJobQueueMock.Object, _loggerServiceMock.Object) {CallBase = true};
        _reportingService = new ReportingService(_powerServiceMock.Object, _recorderServiceMock.Object, _reportJobQueueMock.Object, _loggerServiceMock.Object);
    }
    [Fact]
    public async Task GetDayAheadPosition_WithEmptyResult_ThrowsServiceResultEmptyException()
    {
        // Arrange
        var date = new DateTime(2024, 10, 26);
        IEnumerable<PowerTrade> powerTrades = [];
        _powerServiceMock.Setup(service => service.GetTradesAsync(date))
                         .ReturnsAsync(powerTrades);
                         

        // Act & Assert
        await Assert.ThrowsAsync<ServiceResultEmptyException>(async () => await _reportingService.GetDayAheadPosition(date));
    }

    [Fact]
    public async Task GetDayAheadPosition_WithSuccessResult_ReturnsValidDayAheadPosition()
    {
        // Arrange
        var date = new DateTime(2024, 10, 26);
        IEnumerable<PowerTrade> powerTrades = PowerServiceHelper.GetValidPowerTrades(date, 5);
        _powerServiceMock.Setup(service => service.GetTradesAsync(date))
                         .ReturnsAsync(powerTrades);

        // Act
        var result = await _reportingService.GetDayAheadPosition(date);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(date, result.GetPositionTime());
        Assert.Equal(powerTrades.Count(), result.GetPowerTrades().Count());
    }

    [Fact]
    public async Task ReportingService_ConsumesQueue_FindsAndTriggersDayAheadReport()
    {
        // Arrange
        var job = new DayAheadReportJob{
            Id = Guid.NewGuid(),
            RetryCount = 0,
            TriggerTime = DateTime.Now
        };
        _reportJobQueueMock.SetupSequence(queue => queue.Consume()).Returns(job).Returns((IReportJob)null);;
               
        // Act          
        var task = _reportingServiceMock.Object.SubscribeToJobQueue(TimeSpan.FromSeconds(5));

        // Wait for checking the queue one more time
        await Task.Delay(TimeSpan.FromSeconds(8));


        //Assert
        _loggerServiceMock.Verify(logger => logger.LogInformation("Checking Report Job Queue..."), Times.AtLeastOnce);
        _loggerServiceMock.Verify(logger => logger.LogInformation($"Found a {job.JobType} job in queue."), Times.Once);
        _loggerServiceMock.Verify(logger => logger.LogInformation("Queue is empty."), Times.Once);

        _reportingServiceMock.Verify(s => s.CreateDayAheadReport(job), Times.Once);

    }

    [Fact]
    public async Task GetDayAheadPosition_ServiceThrowsException_ReportPushedToRetry()
    {
        // Arrange
        var triggerDate = DateTime.Now;
        var job = new DayAheadReportJob{
            Id = Guid.NewGuid(),
            RetryCount = 0,
            TriggerTime = triggerDate
        };
        var exception = new PowerServiceException("");
        var reportDate = triggerDate.AddDays(1).Date;
        _powerServiceMock.Setup(service => service.GetTradesAsync(reportDate))
                         .Throws(exception);

        // Act
        await _reportingService.CreateDayAheadReport(job);

        //Assert
        _loggerServiceMock.Verify(logger => logger.LogError($"Exception while creating a report: Type: {job.JobType} Id:{job.Id}", exception), Times.Once);
        _reportJobQueueMock.Verify(s => s.Push(job), Times.Once);
    }

}