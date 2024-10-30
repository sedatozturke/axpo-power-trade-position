using System;

namespace PowerTradePosition.Reporting.Exceptions;

public class ServiceResultEmptyException : Exception
{
    public ServiceResultEmptyException()
        : base("The result from the service was empty.")
    {
    }

    public ServiceResultEmptyException(string message)
        : base(message)
    {
    }

    public ServiceResultEmptyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
