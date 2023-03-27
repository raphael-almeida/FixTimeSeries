using Domain.Entities;

namespace Infrastructure.Interfaces;

/// <summary>
/// Interface for Reading a file and returning a list of FixMessages.
/// </summary>
public interface IFileReader
{
    IEnumerable<FixMessage> ReadFromFile(string path);
}
