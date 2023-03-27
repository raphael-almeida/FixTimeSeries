using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a FIX message within the application.
/// </summary>
public class FixMessage
{
    public MessageType Type { get; set; }
    public DateTime SendingTime { get; set; }
    public EntryType? EntryType { get; set; }
    public string? SecurityId { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
}
