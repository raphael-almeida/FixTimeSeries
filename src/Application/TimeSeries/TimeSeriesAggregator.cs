using System.ComponentModel.Composition;
using Domain.Entities;
using Application.Interfaces;
using Domain.Enums;

namespace Application.TimeSeries;

/// <summary>
/// Implements the logic to aggregate a list of messages into a time series, minute by minute.
/// </summary>
[Export(typeof(ITimeSeriesAggregator))]
public class TimeSeriesAggregator : ITimeSeriesAggregator
{
    /// <summary>
    /// Aggregates a list of messages into a time series, minute by minute.
    /// </summary>
    public IEnumerable<TimeSeriesEntry> Aggregate(IEnumerable<FixMessage> messages)
    {
        if(messages == null || messages.Count() == 0)
        {
            return new List<TimeSeriesEntry>();
        }
        var timeSeriesEntries = new List<TimeSeriesEntry>();
        var filteredMessages = messages.Where(
            m =>
                m.EntryType != null && m.Price != null && m.Quantity != null && m.SecurityId != null
        );
        var groupedMessages = filteredMessages.GroupBy(m => m.SecurityId);
        foreach (var group in groupedMessages)
        {
            var groupEntries = ProcessGroup(group);
            timeSeriesEntries.AddRange(groupEntries);
        }
        return timeSeriesEntries;
    }

    private IEnumerable<TimeSeriesEntry> ProcessGroup(IEnumerable<FixMessage> messages)
    {
        var timeSeriesEntries = new List<TimeSeriesEntry>();
        var messagesGroupedByMinute = messages.GroupBy(
            m => m.SendingTime.ToLocalTime().ToString("HH:mm")
        );
        foreach (var group in messagesGroupedByMinute)
        {
            var bestBid = FindBestBid(group);
            var bestAsk = FindBestAsk(group);
            if (bestBid == null || bestAsk == null)
            {
                continue;
            }

            timeSeriesEntries.Add(
                new TimeSeriesEntry
                {
                    Minute = group.Key,
                    SecurityId = bestBid?.SecurityId,
                    BestBidPrice = bestBid?.Price,
                    BestBidQuantity = bestBid?.Quantity,
                    BestAskPrice = bestAsk?.Price,
                    BestAskQuantity = bestAsk?.Quantity,
                    Spread = bestAsk?.Price - bestBid?.Price
                }
            );
        }
        return timeSeriesEntries;
    }

    /// The best bid is the highest quoted offer price among buyers of a particular security or asset.
    private FixMessage? FindBestBid(IEnumerable<FixMessage> messages)
    {
        var bestBid = messages
            .Where(m => m.EntryType == EntryType.Bid)
            .OrderByDescending(m => m.Price)
            .ThenBy(m => m.SendingTime)
            .FirstOrDefault();
        return bestBid;
    }

    /// The best ask (best offer) refers to the lowest offer price available from among sellers quoting a security.
    private FixMessage? FindBestAsk(IEnumerable<FixMessage> messages)
    {
        var bestAsk = messages
            .Where(m => m.EntryType == EntryType.Ask)
            .OrderBy(m => m.Price)
            .ThenBy(m => m.SendingTime)
            .FirstOrDefault();
        return bestAsk;
    }
}
