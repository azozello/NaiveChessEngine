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

    [Test]
    public void KnightMoveTest()
    {
        var testCases = new List<(string, string)>
        {
            ("8/8/8/8/8/8/8/8", "8/8/8/8/8/8/8/8"),
            ("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKB1R", "8/8/8/8/8/N1N5/8/8"),
            ("8/8/8/8/8/8/8/N7", "8/8/8/8/8/1N6/2N5/8"),
            ("8/8/8/8/8/1N6/8/8", "8/8/8/N1N5/3N4/8/3N4/N1N5"),
            ("8/8/8/N7/8/8/8/8", "8/1N6/2N5/8/2N5/1N6/8/8"),
            ("3N4/5p2/4R3/8/8/8/8/8", "8/1N6/2N5/8/8/8/8/8"),
            ("pppppppp/pPppPPpp/ppPPPppp/pppPNppp/ppPpPPpp/pPpppPpp/pppppppp/pppppppp", "8/8/8/8/8/8/8/8"),
            ("pppppppp/pPp1P1pp/pp1PPp1p/pppPNppp/pp1pPP1p/pPp1p1pp/pppppppp/pppppppp",
                "8/3N1N2/2N3N1/8/2N3N1/3N1N2/8/8"),
        };

        foreach (var tc in testCases)
        {
            var whiteState = Parser.FromFen(tc.Item1);
            var whiteMoves = Parser.FromFen(tc.Item2);

            var blackState = Parser.FromFen(CommonFunctions.InvertFen(tc.Item1));
            var blackMoves = Parser.FromFen(CommonFunctions.InvertFen(tc.Item2));

            var whiteActual = _calculus.KnightMoves(
                whiteState.GetWhitePieces(), whiteState.GetBlackPieces(), whiteState.WhiteKnight);
            var blackActual = _calculus.KnightMoves(
                blackState.GetBlackPieces(), blackState.GetWhitePieces(), blackState.BlackKnight);

            try
            {
                Assert.That(whiteActual, Is.EqualTo(whiteMoves.GetWhitePieces()));
            }
            catch (Exception)
            {
                Console.WriteLine($"{tc.Item1} - {tc.Item1}");
                whiteState.PrintState();
                whiteMoves.PrintState();
                throw;
            }

            try
            {
                Assert.That(blackActual, Is.EqualTo(blackMoves.GetBlackPieces()));
            }
            catch (Exception)
            {
                Console.WriteLine($"{CommonFunctions.InvertFen(tc.Item1)} - {CommonFunctions.InvertFen(tc.Item1)}");
                whiteState.PrintState();
                whiteMoves.PrintState();
                throw;
            }
        }

        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.KnightMoves(65471, 18446462598732840960, 2);
        }

        watch.Stop();
        Console.WriteLine($"MOPS: {1000 / watch.ElapsedMilliseconds}");
    }

    [Test]
    public void BishopMoveTest()
    {
        var testCases = new List<(string, string)>
        {
            ("8/8/8/8/8/8/8/8", "8/8/8/8/8/8/8/8"),
            ("8/8/8/8/8/8/8/B7", "7B/6B1/5B2/4B3/3B4/2B5/1B6/8"),
            ("7B/8/8/8/8/8/8/8", "8/6B1/5B2/4B3/3B4/2B5/1B6/B7"),
            ("8/8/8/8/3B4/8/8/8", "7B/B5B1/1B3B2/2B1B3/8/2B1B3/1B3B2/B5B1"),
            ("8/8/8/2R1r3/3B4/2R1r3/8/8", "8/8/8/8/8/8/8/8"),
            ("8/8/8/2r1r3/3B4/2r1r3/8/8", "8/8/8/8/8/8/8/8"),
            ("8/8/8/2R1R3/3B4/2R1R3/8/8", "8/8/8/8/8/8/8/8"),
            ("8/8/8/8/7B/8/8/8", "3B4/4B3/5B2/6B1/8/6B1/5B2/4B3"),
            ("8/8/B7/8/8/8/8/8", "2B5/1B6/8/1B6/2B5/3B4/4B3/5B2"),
            ("8/8/8/8/8/8/8/4B3", "8/8/8/B7/1B5B/2B3B1/3B1B2/8"),
            ("2B5/8/8/8/8/8/8/8", "8/1B1B4/B3B3/5B2/6B1/7B/8/8"),
            ("pppppppp/pppppppp/pppppppp/pppppppp/pppBpppp/pppppppp/pppppppp/pppppppp", "8/8/8/8/8/8/8/8"),
            ("PPPPPPPP/PPPPPPPP/PPPPPPPP/PPPPPPPP/PPPBPPPP/PPPPPPPP/PPPPPPPP/PPPPPPPP", "8/8/8/8/8/8/8/8"),
            ("PPPPPPP1/1PPPPP1P/P1PPP1PP/PP1P1PPP/PPPBPPPP/PP1P1PPP/P1PPP1PP/1PPPPP1P",
                "7B/B5B1/1B3B2/2B1B3/8/2B1B3/1B3B2/B5B1"),
            ("ppppppp1/1ppppp1p/p1ppp1pp/pp1p1ppp/pppBpppp/pp1p1ppp/p1ppp1pp/1ppppp1p",
                "7B/B5B1/1B3B2/2B1B3/8/2B1B3/1B3B2/B5B1"),
        };

        foreach (var tc in testCases)
        {
            var whiteState = Parser.FromFen(tc.Item1);
            var whiteMoves = Parser.FromFen(tc.Item2);

            var blackState = Parser.FromFen(CommonFunctions.InvertFen(tc.Item1));
            var blackMoves = Parser.FromFen(CommonFunctions.InvertFen(tc.Item2));

            var whiteActual = _calculus.BishopMoves(
                whiteState.GetWhitePieces(), whiteState.GetBlackPieces(), whiteState.WhiteBishop);

            var blackActual = _calculus.BishopMoves(
                blackState.GetBlackPieces(), blackState.GetWhitePieces(), blackState.BlackBishop);

            try
            {
                Assert.That(whiteActual, Is.EqualTo(whiteMoves.GetWhitePieces()));
            }
            catch (Exception)
            {
                Console.WriteLine($"{tc.Item1} - {tc.Item2}");
                whiteState.PrintState();
                whiteMoves.PrintState();
                throw;
            }

            try
            {
                Assert.That(blackActual, Is.EqualTo(blackMoves.GetBlackPieces()));
            }
            catch (Exception)
            {
                Console.WriteLine($"{CommonFunctions.InvertFen(tc.Item1)} - {CommonFunctions.InvertFen(tc.Item2)}");
                whiteState.PrintState();
                whiteMoves.PrintState();
                throw;
            }
        }

        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.BishopMoves(1, 0, 1);
        }

        watch.Stop();
        Console.WriteLine($"MOPS: {1000 / watch.ElapsedMilliseconds}");
    }
}