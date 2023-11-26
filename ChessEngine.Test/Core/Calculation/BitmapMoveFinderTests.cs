using System.Diagnostics;
using ChessEngine.Core.Calculation;
using ChessEngine.Utils;

namespace ChessEngine.Test.Core.Calculation;

public class BitmapMoveFinderTests
{
    private BitmapMoveFinder _calculus;

    [SetUp]
    public void Setup()
    {
        _calculus = new BitmapMoveFinder();
    }

    [Test]
    public void TestTest()
    {
    }

    [Test]
    public void PawnMoveTest()
    {
        var whiteTestCases = new List<(string, string, bool)>
        {
            // White
            ("8/8/8/8/8/8/P7/8", "8/8/8/8/P7/P7/8/8", true),
            ("8/8/8/8/P7/8/8/8", "8/8/8/P7/8/8/8/8", true),
            ("8/8/8/8/8/8/8/P7", "8/8/8/8/8/8/P7/8", true),
            ("8/P7/8/8/8/8/8/8", "P7/8/8/8/8/8/8/8", true),
            ("P7/8/8/8/8/8/8/8", "8/8/8/8/8/8/8/8", true),
            ("8/8/8/8/8/8/7P/8", "8/8/8/8/7P/7P/8/8", true),
            ("8/8/8/8/8/8/8/7P", "8/8/8/8/8/8/7P/8", true),
            ("8/7P/8/8/8/8/8/8", "7P/8/8/8/8/8/8/8", true),
            ("7P/8/8/8/8/8/8/8", "8/8/8/8/8/8/8/8", true),
            ("8/8/8/7P/8/8/8/8", "8/8/7P/8/8/8/8/8", true),
            ("8/8/8/8/8/8/3P4/8", "8/8/8/8/3P4/3P4/8/8", true),
            ("8/8/8/8/3P4/8/8/8", "8/8/8/3P4/8/8/8/8", true),
            ("8/8/8/8/8/3p4/3P4/8", "8/8/8/8/8/8/8/8", true),
            ("8/8/8/8/2p5/8/2P5/8", "8/8/8/8/8/2P5/8/8", true),
            // // Black
            ("8/p7/8/8/8/8/8/8", "8/8/p7/p7/8/8/8/8", false),
            ("8/p7/P7/8/8/8/8/8", "8/8/8/8/8/8/8/8", false),
            ("8/p7/8/P7/8/8/8/8", "8/8/p7/8/8/8/8/8", false),
            ("8/8/p7/8/8/8/8/8", "8/8/8/p7/8/8/8/8", false),
            ("8/8/8/8/8/8/8/p7", "8/8/8/8/8/8/8/8", false),
        };

        foreach (var tc in whiteTestCases)
        {
            var startPosition = Parser.FromFen(tc.Item1);
            var endPosition = Parser.FromFen(tc.Item2);

            var whiteActual = _calculus.PawnMoves(
                own: tc.Item3 ? startPosition.GetWhitePieces() : startPosition.GetBlackPieces(),
                enemy: tc.Item3 ? startPosition.GetBlackPieces() : startPosition.GetWhitePieces(),
                color: tc.Item3,
                piece: tc.Item3 ? startPosition.WhitePawn : startPosition.BlackPawn);

            try
            {
                Assert.That(whiteActual, Is.EqualTo(tc.Item3 ? endPosition.WhitePawn : endPosition.BlackPawn));
            }
            catch (Exception)
            {
                Console.WriteLine($"{tc.Item1} - {tc.Item2}");
                startPosition.PrintState();
                endPosition.PrintState();
                throw;
            }
        }

        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.PawnMoves(256, 0, true, 0);
        }

        watch.Stop();
        Console.WriteLine($"MOPS: {1000 / watch.ElapsedMilliseconds}");
    }
}