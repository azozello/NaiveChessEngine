using ChessEngine.Utils;

namespace ChessEngine.Test.Utils;


public class BitmapChessLoggerTests
{
    private ChessLogger _logger;
    private Dictionary<ulong, List<string>> _testCases;
    private readonly IList<string> _logs = new List<string>();

    private void Log(string data)
    {
        Console.WriteLine(data);
        _logs.Add(data);
    }

    private void ClearLogs()
    {
        Console.WriteLine();
        _logs.Clear();
    }

    [SetUp]
    public void Setup()
    {
        _logger = new ChessLogger(Log);
        _testCases = new Dictionary<ulong, List<string>>
        {
            {
                // Rooks
                0b_10000001_00000000_00000000_00000000_00000000_00000000_00000000_10000001,
                new List<string>
                {
                    "1 0 0 0 0 0 0 1", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0",
                    "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "1 0 0 0 0 0 0 1"
                }
            },
            {
                // Knights
                0b_01000010_00000000_00000000_00000000_00000000_00000000_00000000_01000010,
                new List<string>
                {
                    "0 1 0 0 0 0 1 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0",
                    "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 1 0 0 0 0 1 0"
                }
            },
            {
                // Bishops
                0b_00100100_00000000_00000000_00000000_00000000_00000000_00000000_00100100,
                new List<string>
                {
                    "0 0 1 0 0 1 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0",
                    "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 1 0 0 1 0 0"
                }
            },
            {
                // Pawns
                0b_00000000_11111111_00000000_00000000_00000000_00000000_11111111_00000000,
                new List<string>
                {
                    "0 0 0 0 0 0 0 0", "1 1 1 1 1 1 1 1", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0",
                    "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "1 1 1 1 1 1 1 1", "0 0 0 0 0 0 0 0"
                }
            },
            {
                // Queens 
                0b_00001000_00000000_00000000_00000000_00000000_00000000_00000000_00001000,
                new List<string>
                {
                    "0 0 0 1 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0",
                    "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 1 0 0 0 0"
                }
            },
            {
                // Kings
                0b_00010000_00000000_00000000_00000000_00000000_00000000_00000000_00010000,
                new List<string>
                {
                    "0 0 0 0 1 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0",
                    "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 1 0 0 0"
                }
            }
        };
    }

    [Test]
    public void TestLogBitmap()
    {
        const string text = "0000000000000000000000000000000000000000000000000000000010000001";
        const ulong bitmap = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000001;

        ClearLogs();
        _logger.LogBitmap(bitmap);

        Assert.That(_logs.Count, Is.EqualTo(1));
        Assert.That(_logs[0], Is.EqualTo(text));
    }

    [Test]
    public void TestLogAsBoard()
    {
        foreach (var k in _testCases.Keys)
        {
            ClearLogs();
            _logger.LogAsBoard(k);

            Assert.That(_logs, Has.Count.EqualTo(8));
            for (int i = 0; i < 8; i++)
            {
                Assert.That(_logs[i], Is.EqualTo(_testCases[k][i]));
            }
        }
    }


    [Test]
    public void TestLogAsBoardVerbose()
    {
        var fileNames = "   A B C D E F G H   ";
        foreach (var k in _testCases.Keys)
        {
            ClearLogs();
            _logger.LogAsBoard(k, true);

            Assert.That(_logs, Has.Count.EqualTo(10));
            Assert.That(_logs[0], Is.EqualTo(fileNames));
            Assert.That(_logs[9], Is.EqualTo(fileNames));

            for (int i = 0; i < 8; i++)
            {
                Assert.That(_logs[i + 1], Is.EqualTo($"{8 - i}  {_testCases[k][i]}  {8 - i}"));
            }
        }
    }
}