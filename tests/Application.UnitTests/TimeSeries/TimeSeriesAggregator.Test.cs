using Application.TimeSeries;
using Domain.Entities;
using Domain.Enums;

namespace Application.UnitTests.TimeSeries
{
    [TestFixture]
    public class TimeSeriesAggregatorTests
    {
        private TimeSeriesAggregator _aggregator;

        [SetUp]
        public void SetUp()
        {
            _aggregator = new TimeSeriesAggregator();
        }

        [Test]
        public void Aggregate_Returns_Empty_Enumerable_When_Input_Is_Null()
        {
            // Arrange
            IEnumerable<FixMessage> messages = null;

            // Act
            var result = _aggregator.Aggregate(messages);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void Aggregate_Returns_Empty_Enumerable_When_Input_Is_Empty()
        {
            // Arrange
            var messages = Enumerable.Empty<FixMessage>();

            // Act
            var result = _aggregator.Aggregate(messages);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void Aggregate_Returns_TimeSeriesEntries_When_Input_Is_Valid()
        {
            // Arrange
            var messages = new List<FixMessage>
            {
                new FixMessage
                {
                    SecurityId = "BSA312",
                    EntryType = EntryType.Bid,
                    Price = 100,
                    Quantity = 10
                },
                new FixMessage
                {
                    SecurityId = "BSA312",
                    EntryType = EntryType.Ask,
                    Price = 110,
                    Quantity = 5
                },
                new FixMessage
                {
                    SecurityId = "BSA312",
                    EntryType = EntryType.Bid,
                    Price = 105,
                    Quantity = 15
                },
                new FixMessage
                {
                    SecurityId = "BSA312",
                    EntryType = EntryType.Ask,
                    Price = 115,
                    Quantity = 7
                },
                new FixMessage
                {
                    SecurityId = "BSA315",
                    EntryType = EntryType.Bid,
                    Price = 500,
                    Quantity = 20
                },
                new FixMessage
                {
                    SecurityId = "BSA315",
                    EntryType = EntryType.Ask,
                    Price = 510,
                    Quantity = 12
                }
            };

            // Act
            var result = _aggregator.Aggregate(messages).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.That(
                result.Any(
                    x =>
                        x.SecurityId == "BSA312"
                        && x.BestBidPrice == 100
                        && x.BestBidQuantity == 10
                        && x.BestAskPrice == 110
                        && x.BestAskQuantity == 5
                        && x.Spread == 10
                )
            );
            Assert.That(
                result.Any(
                    x =>
                        x.SecurityId == "BSA312"
                        && x.BestBidPrice == 105
                        && x.BestBidQuantity == 15
                        && x.BestAskPrice == 115
                        && x.BestAskQuantity == 7
                        && x.Spread == 10
                )
            );
            Assert.That(
                result.Any(
                    x =>
                        x.SecurityId == "BSA315"
                        && x.BestBidPrice == 500
                        && x.BestBidQuantity == 20
                        && x.BestAskPrice == 510
                        && x.BestAskQuantity == 12
                        && x.Spread == 10
                )
            );
        }
    }
}
