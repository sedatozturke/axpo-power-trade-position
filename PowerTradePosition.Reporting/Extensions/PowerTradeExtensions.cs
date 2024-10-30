using System;
using System.Globalization;
using Axpo;
using PowerTradePosition.Reporting.Models;

namespace PowerTradePosition.Reporting.Extensions;

public static class PowerTradeExtensions
{
  public static List<string> AsCSVRows<T>(this List<T> powerVolumes) where T : PowerVolume
    {
        if (powerVolumes == null)
            throw new ArgumentNullException(nameof(powerVolumes));
        List<string> csvRows = new List<string>();
        foreach (var item in powerVolumes)
        {
            csvRows.Add($"{item.UTCTime.ToString("yyyy-MM-ddTHH:mm:ssZ")},{item.Volume.ToString(CultureInfo.InvariantCulture)}");
        }
        return csvRows;
    }

    public static List<string> AsCSVRows<T>(this List<T> powerVolumes, string seperator) where T : PowerVolume
    {
        if (powerVolumes == null)
            throw new ArgumentNullException(nameof(powerVolumes));
        List<string> csvRows = new List<string>();
        foreach (var item in powerVolumes)
        {
            csvRows.Add($"{item.UTCTime.ToString("yyyy-MM-ddTHH:mm:ssZ")}{seperator}{item.Volume.ToString(CultureInfo.InvariantCulture)}");
        }
        return csvRows;
    }

    public static List<string> AsCSVRows<T>(this List<T> powerVolumes, string seperator, IFormatProvider formatProvider) where T : PowerVolume
    {
        if (powerVolumes == null)
            throw new ArgumentNullException(nameof(powerVolumes));
        List<string> csvRows = new List<string>();
        foreach (var item in powerVolumes)
        {
            csvRows.Add($"{item.UTCTime.ToString("yyyy-MM-ddTHH:mm:ssZ")}{seperator}{item.Volume.ToString(formatProvider)}");
        }
        return csvRows;
    }

    public static string ToUTCString(this DateTime dateTime)
    {
        return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}
