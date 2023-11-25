using ChessEngine.Core.State;

namespace ChessEngine.Utils;

public class Parser
{
    public static ABoard FromFen(string fen, ulong uPawn = 0)
    {
        ulong cursor = 0b_00000001;

        ulong whitePawn = 0b_00000000;
        ulong whiteKnight = 0b_00000000;
        ulong whiteBishop = 0b_00000000;
        ulong whiteRook = 0b_00000000;
        ulong whiteQueen = 0b_00000000;
        ulong whiteKing = 0b_00000000;

        ulong blackPawn = 0b_00000000;
        ulong blackKnight = 0b_00000000;
        ulong blackBishop = 0b_00000000;
        ulong blackRook = 0b_00000000;
        ulong blackQueen = 0b_00000000;
        ulong blackKing = 0b_00000000;

        var ranks = fen.Split('/').Reverse().ToArray();

        if (ranks.Length != 8)
        {
            throw new ArgumentException("Invalid FEN string: not 8 ranks");
        }

        foreach (var r in ranks)
        {
            foreach (var p in r)
            {
                if (int.TryParse(p.ToString(), out var skip))
                {
                    cursor <<= skip;
                }
                else
                {
                    // (R-Rook, N-Knight, B-Bishop, Q-Queen, K-King, P-Pawn)
                    if (p == 'P') whitePawn |= cursor;
                    else if (p == 'N') whiteKnight |= cursor;
                    else if (p == 'B') whiteBishop |= cursor;
                    else if (p == 'R') whiteRook |= cursor;
                    else if (p == 'Q') whiteQueen |= cursor;
                    else if (p == 'K') whiteKing |= cursor;

                    else if (p == 'p') blackPawn |= cursor;
                    else if (p == 'n') blackKnight |= cursor;
                    else if (p == 'b') blackBishop |= cursor;
                    else if (p == 'r') blackRook |= cursor;
                    else if (p == 'q') blackQueen |= cursor;
                    else if (p == 'k') blackKing |= cursor;
                    cursor <<= 1;
                }
            }
        }

        return new Board(
            whitePawn: whitePawn,
            whiteKnight: whiteKnight,
            whiteBishop: whiteBishop,
            whiteRook: whiteRook,
            whiteQueen: whiteQueen,
            whiteKing: whiteKing,
            blackPawn: blackPawn,
            blackKnight: blackKnight,
            blackBishop: blackBishop,
            blackRook: blackRook,
            blackQueen: blackQueen,
            blackKing: blackKing,
            uPawn: uPawn
        );
    }
}