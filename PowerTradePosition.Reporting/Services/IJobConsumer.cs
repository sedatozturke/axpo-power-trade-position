using System;

namespace PowerTradePosition.Reporting.Services;

public interface IJobConsumer
{
  Task SubscribeToJobQueue(TimeSpan interval);

  void UnsubscribeFromJobQueue();
}
