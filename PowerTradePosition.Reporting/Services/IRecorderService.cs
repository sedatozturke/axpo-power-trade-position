namespace PowerTradePosition.Reporting.Services;

public interface IRecorderService
{
  void WriteAsCSV(string filePath, List<string> data);
  void WriteAsCSV(string filePath, List<string> data, string headerRow);
}
