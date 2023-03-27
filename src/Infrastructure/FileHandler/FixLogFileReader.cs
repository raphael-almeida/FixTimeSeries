using System.ComponentModel.Composition;
using Infrastructure.Interfaces;
using Domain.Entities;

namespace Infrastructure.FileHandler;

/// <summary>
/// This class is responsible for reading a FIX log file and parsing each line into a <see cref="FixMessage"/>.
/// </summary>
[Export(typeof(IFileReader))]
public class FixLogFileReader : IFileReader
{
    [Import(typeof(IParser))]
    public IParser Parser { get; set; }

    /// <summary>
    /// Reads a FIX log file and parses each line into a <see cref="FixMessage"/>.
    /// </summary>
    public IEnumerable<FixMessage> ReadFromFile(string path)
    {
        if(!File.Exists(path))
        {
            throw new FileNotFoundException("Path is a directory.");
        }
        using (var streamReader = new StreamReader(path))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                yield return Parser.ParseLine(line);
            }
        }
    }
}
