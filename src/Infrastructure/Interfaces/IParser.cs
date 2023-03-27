using Domain.Entities;

namespace Infrastructure.Interfaces;

/// <summary>
/// Interface for parsing a line of text into a FixMessage.
/// </summary>
public interface IParser
{
    /// <summary>
    /// Parses a line of log into a FixMessage.
    /// </summary>
    FixMessage ParseLine(string line);
}
