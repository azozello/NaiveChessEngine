using ChessEngine.Utils;

namespace ChessEngine.Test.Utils;

public class ParserTests
{
    [Test]
    public void FromFenTest()
    {
        var testCase = "r1bqkb1r/pppp1ppp/2n2n2/1B2p3/4P3/2N2N2/PPPP1PPP/R1BQK2R";
        var board = Parser.FromFen(testCase.Trim());
        
        board.PrintState();
    }
}