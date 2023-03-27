using System.ComponentModel.Composition;
using Application.Interfaces;
using Infrastructure.Interfaces;

namespace Application;

/// <summary>
/// Implementation of the TimeSeriesService.
/// This class orchestrates the application modules.
/// </summary>
[Export(typeof(ITimeSeriesService))]
public class TimeSeriesService : ITimeSeriesService
{
    [Import(typeof(IFileReader))]
    public IFileReader FileReader { get; set; }

    [Import(typeof(ITimeSeriesAggregator))]
    public ITimeSeriesAggregator TimeSeriesAggregator { get; set; }

    [Import(typeof(IFileWriter))]
    public IFileWriter FileWriter { get; set; }

    /// <summary>
    /// Orchestrates application modules.
    /// </summary>
    public void Run(string path)
    {
        var messages = FileReader.ReadFromFile(path);
        Console.WriteLine($"Read {messages.Count()} messages from file {path}.");
        var timeSeries = TimeSeriesAggregator.Aggregate(messages);
        FileWriter.WriteToFile("time-series.csv", timeSeries);
        Console.WriteLine($"Wrote {timeSeries.Count()} time series entries to file time-series.csv");
    }
}
