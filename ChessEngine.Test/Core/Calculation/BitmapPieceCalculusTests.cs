using System.Diagnostics;
using ChessEngine.Core.Calculation;
using ChessEngine.Core.State;
using ChessEngine.Utils;

namespace ChessEngine.Test.Core.Calculation;

public class BitmapPieceCalculusTests
{
    private BitmapPieceCalculus _calculus;
    private ChessLogger _logger;

    [SetUp]
    public void Setup()
    {
        _calculus = new BitmapPieceCalculus();
        _logger = new ChessLogger(Console.WriteLine);
    }


    [Test]
    public void BishopAttackTest()
    {
        var testCases = new List<(string, string)>
        {
            // North-East
            ("8/8/8/8/8/8/1p6/B7", "8/8/8/8/8/8/1p6/8"),
            ("8/8/8/4p3/8/8/8/B7", "8/8/8/4p3/8/8/8/8"),
            ("7p/8/8/8/8/8/8/B7", "7p/8/8/8/8/8/8/8"),
            ("8/8/8/8/8/2p5/1P6/B7", "8/8/8/8/8/8/8/8"),
            ("8/8/8/4p3/3P4/8/8/B7", "8/8/8/8/8/8/8/8"),
            ("7p/6P1/8/8/8/8/8/B7", "8/8/8/8/8/8/8/8"),
            ("8/8/8/8/8/8/8/B7", "8/8/8/8/8/8/8/8"),

            // South-East
            ("B7/1p6/8/8/8/8/8/8", "8/1p6/8/8/8/8/8/8"),
            ("B7/8/8/3p4/8/8/8/8", "8/8/8/3p4/8/8/8/8"),
            ("B7/8/8/8/8/8/8/7p", "8/8/8/8/8/8/8/7p"),
            ("B7/1P6/2p5/8/8/8/8/8", "8/8/8/8/8/8/8/8"),
            ("B7/8/8/3P4/4p3/8/8/8", "8/8/8/8/8/8/8/8"),
            ("B7/8/8/8/8/8/6P1/7p", "8/8/8/8/8/8/8/8"),
            ("B7/8/8/8/8/8/8/8", "8/8/8/8/8/8/8/8"),

            // South-West
            ("7B/6p1/8/8/8/8/8/8", "8/6p1/8/8/8/8/8/8"),
            ("7B/8/8/8/3p4/8/8/8", "8/8/8/8/3p4/8/8/8"),
            ("7B/8/8/8/8/8/8/p7", "8/8/8/8/8/8/8/p7"),
            ("7B/6P1/5p2/8/8/8/8/8", "8/8/8/8/8/8/8/8"),
            ("7B/8/8/4P3/3p4/8/8/8", "8/8/8/8/8/8/8/8"),
            ("7B/8/8/8/8/8/1P6/p7", "8/8/8/8/8/8/8/8"),
            ("7B/8/8/8/8/8/8/8", "8/8/8/8/8/8/8/8"),

            // North-West
            ("8/8/8/8/8/8/6p1/7B", "8/8/8/8/8/8/6p1/8"),
            ("8/8/8/3p4/8/8/8/7B", "8/8/8/3p4/8/8/8/8"),
            ("p7/8/8/8/8/8/8/7B", "p7/8/8/8/8/8/8/8"),
            ("8/8/8/8/8/5p2/6P1/7B", "8/8/8/8/8/8/8/8"),
            ("8/8/8/3p4/8/5P2/8/7B", "8/8/8/8/8/8/8/8"),
            ("p7/8/8/3P4/8/8/8/7B", "8/8/8/8/8/8/8/8"),
            ("8/8/8/8/8/8/8/7B", "8/8/8/8/8/8/8/8"),

            // Combined
            ("p7/7p/8/8/4B3/3p4/6p1/8", "p7/7p/8/8/8/3p4/6p1/8"),
            ("p3p3/7p/8/8/B7/3p4/6p1/8", "4p3/8/8/8/8/8/8/8"),
            ("r1bqkbnr/pppppppp/n7/8/2B1P3/8/PPPP1PPP/RN1QK1NR", "8/5p2/n7/8/8/8/8/8"),
        };

        foreach (var tc in testCases)
        {
            var whiteBoard = Parser.FromFen(tc.Item1);
            var whiteAttack = Parser.FromFen(tc.Item2);

            var blackBoard = Parser.FromFen(InvertFen(tc.Item1));
            var blackAttack = Parser.FromFen(InvertFen(tc.Item2));

            var whiteExpected = whiteAttack.GetBlackPieces();
            var whiteActual = _calculus.BishopAttack(
                whiteBoard.GetBlackPieces(), whiteBoard.GetWhitePieces(), whiteBoard.WhiteBishop);

            var blackExpected = blackAttack.GetWhitePieces();
            var blackActual = _calculus.BishopAttack(
                blackBoard.GetWhitePieces(), blackBoard.GetBlackPieces(), blackBoard.BlackBishop);

            Assert.That(whiteActual, Is.EqualTo(whiteExpected));
            Assert.That(blackActual, Is.EqualTo(blackExpected));
        }

        var watch = new Stopwatch();
        var board = Parser.FromFen("p7/7p/8/8/4B3/3p4/6p1/8");
        watch.Start();

        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.BishopAttack(board.GetBlackPieces(), board.GetWhitePieces(), board.WhiteBishop);
        }

        watch.Stop();
        Console.WriteLine($"MOPS: {1000 / watch.ElapsedMilliseconds}");
    }

    [Test]
    public void KnightAttackTest()
    {
        var whiteTestCases = new List<(string, string)>
        {
            ("8/8/8/8/8/1p6/2p5/N7", "8/8/8/8/8/1p6/2p5/8"),
            ("8/8/8/8/p1p5/3p4/1N6/3p4", "8/8/8/8/p1p5/3p4/8/3p4"),
            ("8/8/8/1p1p4/p3p3/2N5/p3p3/1p1p4", "8/8/8/1p1p4/p3p3/8/p3p3/1p1p4"),

            ("4p1p1/3p3p/5N2/3p3p/4p1p1/8/8/8", "4p1p1/3p3p/8/3p3p/4p1p1/8/8/8"),
            ("4p3/6N1/4p3/5p1p/8/8/8/8", "4p3/8/4p3/5p1p/8/8/8/8"),
            ("7N/5p2/6p1/8/8/8/8/8", "8/5p2/6p1/8/8/8/8/8"),

            ("N7/2p5/1p6/8/8/8/8/8", "8/2p5/1p6/8/8/8/8/8"),
            ("3p4/1N6/3p4/p1p5/8/8/8/8", "3p4/8/3p4/p1p5/8/8/8/8"),
            ("1p1p4/p3p3/2N5/p3p3/1p1p4/8/8/8", "1p1p4/p3p3/8/p3p3/1p1p4/8/8/8"),

            ("8/8/8/8/8/6p1/5p2/7N", "8/8/8/8/8/6p1/5p2/8"),
            ("8/8/8/8/5p1p/4p3/6N1/4p3", "8/8/8/8/5p1p/4p3/8/4p3"),
            ("8/8/8/4p1p1/3p3p/5N2/3p3p/4p1p1", "8/8/8/4p1p1/3p3p/8/3p3p/4p1p1"),

            ("8/8/2p1P3/8/1P1N4/5p2/2p1P3/8", "8/8/2p5/8/8/5p2/2p5/8"),
            ("pppppppp/ppp1p1pp/pp1ppp1p/ppppNppp/pp1ppp1p/ppp1p1pp/pppppppp/pppppppp", "8/8/8/8/8/8/8/8"),
            ("4p3/3ppp2/2pPpPp1/1pPpppPp/ppppNppp/1pPpppPp/2pPpPp1/3ppp2", "8/8/8/8/8/8/8/8"),

            ("8/8/8/8/1p6/2p5/N7/2p5", "8/8/8/8/1p6/2p5/8/2p5"),
            ("8/8/8/1p6/2p5/N7/2p5/1p6", "8/8/8/1p6/2p5/8/2p5/1p6"),
            ("2p5/N7/2p5/1p6/8/8/8/8", "2p5/8/2p5/1p6/8/8/8/8"),
            ("1p6/2p5/N7/2p5/1p6/8/8/8", "1p6/2p5/8/2p5/1p6/8/8/8"),

            ("8/8/8/8/8/p1p5/3p4/1N6", "8/8/8/8/8/p1p5/3p4/8"),
            ("8/8/8/8/8/1p1p4/p3p3/2N5", "8/8/8/8/8/1p1p4/p3p3/8"),
            ("8/8/8/8/8/5p1p/4p3/6N1", "8/8/8/8/8/5p1p/4p3/8"),
            ("8/8/8/8/8/4p1p1/3p3p/5N2", "8/8/8/8/8/4p1p1/3p3p/8"),

            ("1N6/3p4/p1p5/8/8/8/8/8", "8/3p4/p1p5/8/8/8/8/8"),
            ("2N5/p3p3/1p1p4/8/8/8/8/8", "8/p3p3/1p1p4/8/8/8/8/8"),
            ("6N1/4p3/5p1p/8/8/8/8/8", "8/4p3/5p1p/8/8/8/8/8"),
            ("5N2/3p3p/4p1p1/8/8/8/8/8", "8/3p3p/4p1p1/8/8/8/8/8"),

            ("5p2/7N/5p2/6p1/8/8/8/8", "5p2/8/5p2/6p1/8/8/8/8"),
            ("6p1/5p2/7N/5p2/6p1/8/8/8", "6p1/5p2/8/5p2/6p1/8/8/8"),
            ("8/8/8/8/6p1/5p2/7N/5p2", "8/8/8/8/6p1/5p2/8/5p2"),
            ("8/8/8/6p1/5p2/7N/5p2/6p1", "8/8/8/6p1/5p2/8/5p2/6p1"),

            ("8/8/8/p1p5/3p4/1N6/3p4/p1p5", "8/8/8/p1p5/3p4/8/3p4/p1p5"),
            ("8/8/8/8/1p1p4/p3p3/2N5/p3p3", "8/8/8/8/1p1p4/p3p3/8/p3p3"),

            ("p1p5/3p4/1N6/3p4/p1p5/8/8/8", "p1p5/3p4/8/3p4/p1p5/8/8/8"),
            ("p3p3/2N5/p3p3/1p1p4/8/8/8/8", "p3p3/8/p3p3/1p1p4/8/8/8/8"),

            ("3p3p/5N2/3p3p/4p1p1/8/8/8/8", "3p3p/8/3p3p/4p1p1/8/8/8/8"),
            ("5p1p/4p3/6N1/4p3/5p1p/8/8/8", "5p1p/4p3/8/4p3/5p1p/8/8/8"),

            ("8/8/8/8/4p1p1/3p3p/5N2/3p3p", "8/8/8/8/4p1p1/3p3p/8/3p3p"),
            ("8/8/8/5p1p/4p3/6N1/4p3/5p1p", "8/8/8/5p1p/4p3/8/4p3/5p1p"),
        };

        foreach (var tc in whiteTestCases)
        {
            var whiteBoard = Parser.FromFen(tc.Item1);
            var whiteAttack = Parser.FromFen(tc.Item2);

            var blackBoard = Parser.FromFen(InvertFen(tc.Item1));
            var blackAttack = Parser.FromFen(InvertFen(tc.Item2));

            var whiteExpected = whiteAttack.GetBlackPieces();
            var whiteActual = _calculus.KnightAttack(whiteBoard.GetBlackPieces(), whiteBoard.WhiteKnight);

            var blackExpected = blackAttack.GetWhitePieces();
            var blackActual = _calculus.KnightAttack(blackBoard.GetWhitePieces(), blackBoard.BlackKnight);

            Assert.That(whiteActual, Is.EqualTo(whiteExpected));
            Assert.That(blackActual, Is.EqualTo(blackExpected));
        }

        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.KnightAttack(
                0b_0000000000000000000000000000000000000000000000100000010000000000,
                0b_0000000000000000000000000000000000000000000000000000000000000001);
        }

        watch.Stop();
        Console.WriteLine($"MOPS: {1000 / watch.ElapsedMilliseconds}");
    }

    private string InvertFen(string fen)
    {
        string invertedFen = "";
        foreach (var c in fen)
        {
            if (c == 'p') invertedFen += "P";
            else if (c == 'n') invertedFen += "N";
            else if (c == 'b') invertedFen += "B";
            else if (c == 'r') invertedFen += "R";
            else if (c == 'q') invertedFen += "Q";
            else if (c == 'k') invertedFen += "K";

            else if (c == 'P') invertedFen += "p";
            else if (c == 'N') invertedFen += "n";
            else if (c == 'B') invertedFen += "b";
            else if (c == 'R') invertedFen += "r";
            else if (c == 'Q') invertedFen += "q";
            else if (c == 'K') invertedFen += "k";

            else invertedFen += c;
        }

        return invertedFen;
    }

    [Test]
    public void TestRookAttack()
    {
        var board = new Board(
            whitePawn: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            whiteKnight: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            whiteBishop: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010,
            whiteRook: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001,
            whiteQueen: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            whiteKing: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            blackPawn: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            blackKnight: 0b_00000000_00000000_00000000_00000000_00000000_00000001_00000000_00000100,
            blackBishop: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            blackRook: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            blackQueen: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
            blackKing: 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000
        );
        // 1 - Rook
        // 2 - Enemy Piece
        // 3 - Own Piece
        // 0 - Empty Square

        /*
         *    A B C D E F G H
           8  0 0 0 0 0 0 0 0  8
           7  0 0 0 0 0 0 0 0  7
           6  0 0 0 0 0 0 0 0  6
           5  0 0 0 0 0 0 0 0  5
           4  0 0 0 0 0 0 0 0  4
           3  2 0 0 0 0 0 0 0  3
           2  0 0 0 0 0 0 0 0  2
           1  1 3 2 0 0 0 0 0  1
         *    A B C D E F G H
         */
        board.PrintState();
        var underAttack = _calculus.PiecesAttackedByRooks(board, true);
        _logger.LogBitmap(underAttack);
    }

    [Test]
    public void PawnAttackTest()
    {
        var whiteTestCases = new List<(string, ulong)>
        {
            ("8/8/8/8/8/8/1p6/P7", 0b_00000010_00000000),
            ("8/8/8/8/8/8/8/P7", 0b_00000000),
            ("8/8/8/8/8/8/6p1/7P", 0b_0000000000000000000000000000000000000000000000000100000000000000),
            ("8/8/8/8/3p4/8/8/7P", 0b_00000000),
            ("8/8/8/8/1p1p4/2P5/8/8", 0b_0000000000000000000000000000000000001010000000000000000000000000),
            ("8/8/8/5p2/1p6/2P5/8/8", 0b_0000000000000000000000000000000000000010000000000000000000000000),
            ("8/8/2p5/5p2/8/2P5/8/8", 0b_00000000),
            ("p1p5/1P6/8/8/8/8/8/8", 0b_0000010100000000000000000000000000000000000000000000000000000000),
            ("p3p3/1P6/8/8/8/8/8/8", 0b_0000000100000000000000000000000000000000000000000000000000000000),
            ("4p1p1/1P6/8/8/8/8/8/8", 0b_00000000),
        };

        var blackTestCases = new List<(string, ulong)>
        {
            ("p7/1P6/8/8/8/8/8/8", 0b_0000000000000010000000000000000000000000000000000000000000000000),
            ("8/8/8/p7/1P6/8/8/8", 0b_0000000000000000000000000000000000000010000000000000000000000000),
            ("8/8/8/p7/8/2P5/8/8", 0),
            ("7p/6P1/8/8/8/8/8/8", 0b_0000000001000000000000000000000000000000000000000000000000000000),
            ("8/8/8/8/7p/6P1/8/8", 0b_0000000000000000000000000000000000000000010000000000000000000000),
            ("8/8/8/8/3P3p/8/8/8", 0),
            ("8/8/8/4p3/3P4/8/8/8", 0b_0000000000000000000000000000000000001000000000000000000000000000),
            ("8/8/8/4p3/3P1P2/8/8/8", 0b_0000000000000000000000000000000000101000000000000000000000000000),
            ("8/8/8/4p3/5P2/3P4/8/8", 0b_0000000000000000000000000000000000100000000000000000000000000000),
            ("8/8/8/3PpP2/8/8/8/8", 0),
        };

        var enPassantTestCases = new List<(string, ulong, ulong, bool)>
        {
            // white
            (
                "8/8/8/8/2pPp3/8/8/8",
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                true
            ), // U-right
            (
                "8/8/8/8/2pPp3/8/8/8",
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                true
            ), // U-left
            (
                "8/8/8/2p5/3Pp3/8/8/8",
                0b_0000000000000000000000000000010000010000000000000000000000000000,
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                true
            ), // U-right
            (
                "8/8/8/4p3/2pP4/8/8/8",
                0b_0000000000000000000000000001000000000100000000000000000000000000,
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                true
            ), // U-left
            (
                "8/1p6/8/8/3Pp3/8/8/8",
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                true
            ), // U-right
            (
                "8/6p1/8/8/2pP4/8/8/8",
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                true
            ), // U-left
            // Black
            (
                "8/8/8/8/2PpP3/8/8/8",
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                false
            ), // U-right
            (
                "8/8/8/8/2PpP3/8/8/8",
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                false
            ), // U-left
            (
                "8/8/8/8/3pP3/2P5/8/8",
                0b_0000000000000000000000000000000000010000000001000000000000000000,
                0b_0000000000000000000000000000000000010000000000000000000000000000,
                false
            ), // U-right
            (
                "8/8/8/8/2Pp4/4P3/8/8",
                0b_0000000000000000000000000000000000000100000100000000000000000000,
                0b_0000000000000000000000000000000000000100000000000000000000000000,
                false
            ), // U-left
            (
                "8/1P6/8/8/4pP2/8/8/8",
                0b_0000000000000000000000000000000000100000000000000000000000000000,
                0b_0000000000000000000000000000000000100000000000000000000000000000,
                false
            ), // U-right
            (
                "8/6P1/8/8/1Pp5/8/8/8",
                0b_0000000000000000000000000000000000000010000000000000000000000000,
                0b_0000000000000000000000000000000000000010000000000000000000000000,
                false
            ), // U-left
        };

        foreach (var tc in whiteTestCases)
        {
            var tempBoard = Parser.FromFen(tc.Item1);
            ulong actual = _calculus.PawnAttack(tempBoard.GetBlackPieces(), tempBoard.WhitePawn, true, 0);
            Assert.That(actual, Is.EqualTo(tc.Item2));
        }

        foreach (var tc in blackTestCases)
        {
            var tempBoard = Parser.FromFen(tc.Item1);
            ulong actual = _calculus.PawnAttack(tempBoard.GetWhitePieces(), tempBoard.BlackPawn, false, 0);
            Assert.That(actual, Is.EqualTo(tc.Item2));
        }

        foreach (var tc in enPassantTestCases)
        {
            var tempBoard = Parser.FromFen(tc.Item1, tc.Item3);
            var actual = tc.Item4
                ? _calculus.PawnAttack(tempBoard.GetBlackPieces(), tempBoard.WhitePawn, true, tc.Item3)
                : _calculus.PawnAttack(tempBoard.GetWhitePieces(), tempBoard.BlackPawn, false, tc.Item3);
            Assert.That(actual, Is.EqualTo(tc.Item2));
        }

        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.PawnAttack(
                0b_0000000000000000000000000000000000010100000000000000000000000000,
                0b_0000000000000000000000000000000000001000000000000000000000000000,
                true,
                0b_0000000000000000000000000000000000010000000000000000000000000000);
        }

        watch.Stop();
        Console.WriteLine($"MOPS: {1000 / watch.ElapsedMilliseconds}");
    }

    /// <summary>
    /// Test sliding piece attack on a same rank.
    /// </summary>
    [Test]
    public void SlidingPieceRankAttackTest()
    {
        var testCases = new List<List<ulong>>
        {
            // GetEnemyPieceAttackedOnALeft
            new() { 0b_00000001, 0b_00000010, 0b_00000001, 0b_00000010 }, //  1
            new() { 0b_00000001, 0b_00100000, 0b_00000001, 0b_00100000 }, //  2
            new() { 0b_00000001, 0b_10000000, 0b_00000001, 0b_10000000 }, //  3

            new() { 0b_00000011, 0b_00000100, 0b_00000001, 0b_00000000 }, //  4
            new() { 0b_00010001, 0b_00100000, 0b_00000001, 0b_00000000 }, //  5
            new() { 0b_01000001, 0b_10000000, 0b_00000001, 0b_00000000 }, //  6

            new() { 0b_00000000, 0b_00000000, 0b_00000001, 0b_00000000 }, //  7
            new() { 0b_00000001, 0b_00000000, 0b_00000001, 0b_00000000 }, //  8
            new() { 0b_00000000, 0b_11111110, 0b_00000001, 0b_00000000 }, //  9

            new() { 0b_11111110, 0b_00000000, 0b_00000001, 0b_00000000 }, // 10
            new() { 0b_11111110, 0b_11111110, 0b_00000001, 0b_00000000 }, // 11

            // GetEnemyPieceAttackedOnARight
            new() { 0b_10000000, 0b_00000001, 0b_10000000, 0b_00000001 }, // 12
            new() { 0b_10000000, 0b_00001000, 0b_10000000, 0b_00001000 }, // 13
            new() { 0b_10000000, 0b_01000000, 0b_10000000, 0b_01000000 }, // 14

            new() { 0b_10000001, 0b_00000010, 0b_10000000, 0b_00000010 }, // 15
            new() { 0b_10000100, 0b_00001000, 0b_10000000, 0b_00001000 }, // 16
            new() { 0b_10100000, 0b_01000000, 0b_10000000, 0b_01000000 }, // 17

            new() { 0b_10000010, 0b_00000001, 0b_10000000, 0b_00000000 }, // 18
            new() { 0b_10001000, 0b_00000100, 0b_10000000, 0b_00000000 }, // 19
            new() { 0b_11000000, 0b_00100000, 0b_10000000, 0b_00000000 }, // 20

            // GetEnemyPieceAttackedOnABothSides
            new() { 0b_00010000, 0b_00001000, 0b_00010000, 0b_00001000 }, // 20
            new() { 0b_00010000, 0b_00000010, 0b_00010000, 0b_00000010 }, // 21
            new() { 0b_00010000, 0b_00000001, 0b_00010000, 0b_00000001 }, // 22

            new() { 0b_00010000, 0b_00100000, 0b_00010000, 0b_00100000 }, // 23
            new() { 0b_00010000, 0b_01000000, 0b_00010000, 0b_01000000 }, // 24
            new() { 0b_00010000, 0b_10000000, 0b_00010000, 0b_10000000 }, // 25

            new() { 0b_00010000, 0b_00101000, 0b_00010000, 0b_00101000 }, // 26
            new() { 0b_00010000, 0b_01000100, 0b_00010000, 0b_01000100 }, // 27
            new() { 0b_00010000, 0b_10000001, 0b_00010000, 0b_10000001 }, // 28

            new() { 0b_00010100, 0b_00000010, 0b_00010000, 0b_00000000 }, // 29
            new() { 0b_00010010, 0b_00000001, 0b_00010000, 0b_00000000 }, // 30

            new() { 0b_00110000, 0b_01000000, 0b_00010000, 0b_00000000 }, // 31
            new() { 0b_01010000, 0b_10000000, 0b_00010000, 0b_00000000 }, // 32

            new() { 0b_00110100, 0b_01000010, 0b_00010000, 0b_00000000 }, // 33
            new() { 0b_01010010, 0b_10000001, 0b_00010000, 0b_00000000 } // 34
        };

        var index = 0;
        foreach (var c in testCases)
        {
            index++;
            var actual = _calculus.SlidingPieceRankAttack(c[0], c[1], c[2]);
            try
            {
                Assert.That(actual, Is.EqualTo(c[3]));
            }
            catch
            {
                _logger.LogBitmap(c[0]);
                _logger.LogBitmap(c[1]);
                _logger.LogBitmap(c[2]);
                _logger.LogBitmap(c[3]);
                _logger.LogBitmap(actual);
                Console.WriteLine($"Test case: {index}");
                throw;
            }
        }

        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.SlidingPieceRankAttack(0b_00010000, 0b_10000001, 0b_00010000);
        }

        watch.Stop();
        Console.WriteLine($"MOPS: {1000 / watch.ElapsedMilliseconds}");
    }
}