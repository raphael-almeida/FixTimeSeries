
using Domain.Entities;

namespace Infrastructure.Interfaces;

/// <summary>
/// Interface for writing a list of TimeSeriesEntries to a file.
/// </summary>
public interface IFileWriter
{
    /// <summary>
    /// Writes a list of TimeSeriesEntries to a file.
    /// </summary>
    void WriteToFile(string path, IEnumerable<TimeSeriesEntry> timeSeriesEntries);
}