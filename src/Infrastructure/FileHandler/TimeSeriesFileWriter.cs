using System.ComponentModel.Composition;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.FileHandler;

/// <summary>
/// This class is responsible for writing a time series to a CSV file.
/// </summary>
[Export(typeof(IFileWriter))]
public class TimeSeriesFileWriter : IFileWriter
{
    /// <summary>
    /// Writes a time series to a CSV file.
    /// </summary>
    public void WriteToFile(string path, IEnumerable<TimeSeriesEntry> timeSeriesEntries)
    {
        using (var streamWriter = new StreamWriter(path))
        {
            streamWriter.WriteLine("Minute,SecID,Best Bid,Best Ask,Spread");
            foreach (var timeSeriesEntry in timeSeriesEntries)
            {
                streamWriter.WriteLine(FormatLine(timeSeriesEntry));
            }
        }
    }

    private string FormatLine(TimeSeriesEntry timeSeriesEntry)
    {
        return $"{timeSeriesEntry.Minute},{timeSeriesEntry.SecurityId},{timeSeriesEntry.BestBidPrice} - {timeSeriesEntry.BestBidQuantity} units,{timeSeriesEntry.BestAskPrice} - {timeSeriesEntry.BestBidQuantity} units,{timeSeriesEntry.Spread}";
    }
}
