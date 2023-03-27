namespace Domain.Entities
{
    /// <summary>
    /// Represents a single entry in a time series
    /// </summary>
    public class TimeSeriesEntry
    {
        public string? Minute { get; set; }
        public string? SecurityId { get; set; }
        public decimal? BestBidPrice { get; set; }
        public int? BestBidQuantity { get; set; }
        public decimal? BestAskPrice { get; set; }
        public int? BestAskQuantity { get; set; }
        public decimal? Spread { get; set; }
    }
}
