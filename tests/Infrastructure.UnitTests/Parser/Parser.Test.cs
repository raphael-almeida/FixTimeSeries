using NUnit.Framework;
using Infrastructure.Parser;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.UnitTests;

public class ParserTests
{
    private FixParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new FixParser();
    }

    [TestCase(
        "10:26:15.161: 656308 - 81 - 1128=935=X34=65630852=20140610-13:26:15.15775=20140610268=1279=0269=122=8207=BVMF48=380977983=20179270=7.41273=61:51:57271=100272=2014061037016=2014061037017=61:51:5737=80531934678289=8290=6"
    )]
    public void Parser_ParsesSingleLine(string line)
    {
        var message = _parser.ParseLine(line);
        Console.WriteLine(message.Quantity.ToString());
        Assert.That(message.Type, Is.EqualTo(MessageType.MarketData));
        Assert.That(message.SendingTime, Is.EqualTo(new DateTime(2014, 06, 10, 13, 26, 15, 157)));
        Assert.That(message.EntryType, Is.EqualTo(EntryType.Ask));
        Assert.That(message.SecurityId, Is.EqualTo("3809779"));
        Assert.That(message.Price, Is.EqualTo(7.41m));
        Assert.That(message.Quantity, Is.EqualTo(100));
    }

    [TestCase(
        "10:26:15.161: 656308 - 81 - 1128=935=X34=65630852=20140610-13:26:15.15775=20140610268=1279=0269=122=8207=BVMF48=380977983=20179270=7.41273=61:51:57271=100272=2014061037016=2014061037017=61:51:5737=80531934678289=8290=6",
        "10:26:15.161: 656308 - 81 - 1128=935=X34=65630852=20140610-13:26:15.15775=20140610268=1279=0269=122=8207=BVMF48=380977983=20179270=7.41273=61:51:57271=100272=2014061037016=2014061037017=61:51:5737=80531934678289=8290=6"
    )]
    public void Parser_ParsesMultipleLines(string line1, string line2)
    {
        var message1 = _parser.ParseLine(line1);
        var message2 = _parser.ParseLine(line2);
        Assert.That(message1.Type, Is.EqualTo(MessageType.MarketData));
        Assert.That(message1.SendingTime, Is.EqualTo(new DateTime(2014, 06, 10, 13, 26, 15, 157)));
        Assert.That(message1.EntryType, Is.EqualTo(EntryType.Ask));
        Assert.That(message1.SecurityId, Is.EqualTo("3809779"));
        Assert.That(message1.Price, Is.EqualTo(7.41m));
        Assert.That(message1.Quantity, Is.EqualTo(100));
        Assert.That(message2.Type, Is.EqualTo(MessageType.MarketData));
        Assert.That(message2.SendingTime, Is.EqualTo(new DateTime(2014, 06, 10, 13, 26, 15, 157)));
        Assert.That(message2.EntryType, Is.EqualTo(EntryType.Ask));
        Assert.That(message2.SecurityId, Is.EqualTo("3809779"));
        Assert.That(message2.Price, Is.EqualTo(7.41m));
        Assert.That(message2.Quantity, Is.EqualTo(100));
    }
}
