using Moq;
using Infrastructure.Interfaces;

namespace Infrastructure.FileHandler.Tests
{
    [TestFixture]
    public class FixLogFileReaderTests
    {
        private FixLogFileReader _fixLogFileReader;
        private Mock<IParser> _parserMock;

        [SetUp]
        public void Setup()
        {
            _parserMock = new Mock<IParser>();
            _fixLogFileReader = new FixLogFileReader { Parser = _parserMock.Object };
        }

        [Test]
        public void ReadFromFile_WhenFilePathIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            string filePath = null;

            // Act & Assert
            try
            {
                _fixLogFileReader.ReadFromFile(filePath);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf<ArgumentNullException>(e);
                Assert.AreEqual("Value cannot be null. (Parameter 'path')", e.Message);
            }
        }

        [Test]
        public void ReadFromFile_WhenFileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            string filePath = "invalid_file_path.txt";

            // Act & Assert
            try
            {
                _fixLogFileReader.ReadFromFile(filePath);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf<FileNotFoundException>(e);
                Assert.AreEqual("Value cannot be null. (Parameter 'path')", e.Message);
            }
        }
    }
}
