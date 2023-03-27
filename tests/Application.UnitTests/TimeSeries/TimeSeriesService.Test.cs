using Application;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace Application.Tests
{
    [TestFixture]
    public class TimeSeriesServiceTests
    {
        private TimeSeriesService timeSeriesService;
        private Mock<IFileReader> fileReaderMock;
        private Mock<ITimeSeriesAggregator> timeSeriesAggregatorMock;
        private Mock<IFileWriter> fileWriterMock;

        [SetUp]
        public void SetUp()
        {
            fileReaderMock = new Mock<IFileReader>();
            timeSeriesAggregatorMock = new Mock<ITimeSeriesAggregator>();
            fileWriterMock = new Mock<IFileWriter>();

            timeSeriesService = new TimeSeriesService
            {
                FileReader = fileReaderMock.Object,
                TimeSeriesAggregator = timeSeriesAggregatorMock.Object,
                FileWriter = fileWriterMock.Object
            };
        }

        [Test]
        public void Run_ShouldReadMessagesFromFile()
        {
            // Arrange
            var filePath = "test-file.txt";
            var messages = new List<FixMessage>();
            fileReaderMock.Setup(x => x.ReadFromFile(filePath)).Returns(messages);

            // Act
            timeSeriesService.Run(filePath);

            // Assert
            fileReaderMock.Verify(x => x.ReadFromFile(filePath), Times.Once);
        }

        [Test]
        public void Run_ShouldAggregateTimeSeries()
        {
            // Arrange
            var filePath = "test-file.txt";
            var messages = new List<FixMessage>();
            var timeSeries = new List<TimeSeriesEntry>();
            fileReaderMock.Setup(x => x.ReadFromFile(filePath)).Returns(messages);
            timeSeriesAggregatorMock.Setup(x => x.Aggregate(messages)).Returns(timeSeries);

            // Act
            timeSeriesService.Run(filePath);

            // Assert
            timeSeriesAggregatorMock.Verify(x => x.Aggregate(messages), Times.Once);
        }

        [Test]
        public void Run_ShouldWriteTimeSeriesToFile()
        {
            // Arrange
            var filePath = "test-file.txt";
            var messages = new List<FixMessage>();
            var timeSeries = new List<TimeSeriesEntry>();
            fileReaderMock.Setup(x => x.ReadFromFile(filePath)).Returns(messages);
            timeSeriesAggregatorMock.Setup(x => x.Aggregate(messages)).Returns(timeSeries);

            // Act
            timeSeriesService.Run(filePath);

            // Assert
            fileWriterMock.Verify(x => x.WriteToFile("time-series.csv", timeSeries), Times.Once);
        }
    }
}
