using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Interface for the TimeSeriesAggregator
/// </summary>
public interface ITimeSeriesAggregator
{
    /// <summary>
    /// Aggregates a list of messages into a time series
    /// </summary>
    IEnumerable<TimeSeriesEntry> Aggregate(IEnumerable<FixMessage> messages);
}
