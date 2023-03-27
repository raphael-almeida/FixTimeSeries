using System.Globalization;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;
using System.ComponentModel.Composition;

namespace Infrastructure.Parser;

/// <summary>
/// This class is responsible for parsing a line of text into a <see cref="FixMessage"/>.
/// </summary>
[Export(typeof(IParser))]
public class FixParser : IParser
{
    private const byte DELIMITER = 01;
    private const string MESSAGE_TYPE = "35";
    private const string SENDING_TIME = "52";
    private const string ENTRY_TYPE = "269";
    private const string SECURITY_ID = "48";
    private const string PRICE = "270";
    private const string QUANTITY = "271";

    /// <summary>
    /// Parses a single line of log into a <see cref="FixMessage"/>.
    /// </summary>
    public FixMessage ParseLine(string line)
    {
        var fields = SplitToDictionary(SanitizeMessage(line));
        return new FixMessage
        {
            Type = ParseMessageType(fields),
            SendingTime = ParseSendingTime(fields),
            EntryType = ParseEntryType(fields),
            SecurityId = ParseSecurityId(fields),
            Price = ParsePrice(fields),
            Quantity = ParseQuantity(fields)
        };
    }

    private string SanitizeMessage(string line)
    {
        const string startIdentifier = "1128=";
        var index = line.IndexOf(startIdentifier);
        line = line.Substring(index);
        if (line.EndsWith(Convert.ToChar(DELIMITER)))
        {
            line = line.Substring(0, line.Length - 1);
        }
        return line;
    }

    private Dictionary<string, string> SplitToDictionary(string line)
    {
        var fields = line.Split(Convert.ToChar(DELIMITER));
        var dictionary = new Dictionary<string, string>();
        foreach (var item in fields)
        {
            var keyValue = item.Split('=');
            if (SkipKey(keyValue, dictionary))
            {
                continue;
            }
            dictionary.Add(keyValue[0], keyValue[1]);
        }
        return dictionary;
    }

    private bool SkipKey(string[] keyValue, Dictionary<string, string> dictionary)
    {
        string key = keyValue[0];
        var validKeys = new HashSet<string>
        {
            MESSAGE_TYPE,
            SENDING_TIME,
            ENTRY_TYPE,
            SECURITY_ID,
            PRICE,
            QUANTITY
        };
        if (!validKeys.Contains(key))
        {
            return true;
        }
        if (keyValue.Length != 2)
        {
            return true;
        }
        if (dictionary.ContainsKey(keyValue[0]))
        {
            return true;
        }

        return false;
    }

    private MessageType ParseMessageType(Dictionary<string, string> fields)
    {
        return fields[MESSAGE_TYPE] switch
        {
            "X" => MessageType.MarketData,
            _ => throw new NotImplementedException()
        };
    }

    private DateTime ParseSendingTime(Dictionary<string, string> fields)
    {
        return DateTime.ParseExact(
            fields[SENDING_TIME],
            "yyyyMMdd-HH:mm:ss.fff",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
        );
    }

    private EntryType? ParseEntryType(Dictionary<string, string> fields)
    {
        return fields[ENTRY_TYPE] switch
        {
            "0" => EntryType.Bid,
            "1" => EntryType.Ask,
            _ => null
        };
    }

    private string ParseSecurityId(Dictionary<string, string> fields)
    {
        return fields[SECURITY_ID];
    }

    private decimal? ParsePrice(Dictionary<string, string> fields)
    {
        return fields.ContainsKey(PRICE) ? decimal.Parse(fields[PRICE]) : null;
    }

    private int? ParseQuantity(Dictionary<string, string> fields)
    {
        return fields.ContainsKey(QUANTITY) ? int.Parse(fields[QUANTITY]) : null;
    }
}
