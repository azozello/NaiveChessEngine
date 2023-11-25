namespace ChessEngine.Core.State;

public enum PieceFile
{
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H
}

public enum PieceRank
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
}

// Ranks mapping - little-endian (1 < 8)
// File mapping  - little-endian (A < H)     
public class Board : ABoard
{
    public Board(
        ulong whitePawn, ulong whiteKnight, ulong whiteBishop, ulong whiteRook, ulong whiteQueen, ulong whiteKing,
        ulong blackPawn, ulong blackKnight, ulong blackBishop, ulong blackRook, ulong blackQueen, ulong blackKing,
        ulong uPawn = 0, ulong castle = 0b_10000001_00000000_00000000_00000000_00000000_00000000_00000000_10000001)
    {
        WhitePawn = whitePawn;
        WhiteKnight = whiteKnight;
        WhiteBishop = whiteBishop;
        WhiteRook = whiteRook;
        WhiteQueen = whiteQueen;
        WhiteKing = whiteKing;
        BlackPawn = blackPawn;
        BlackKnight = blackKnight;
        BlackBishop = blackBishop;
        BlackRook = blackRook;
        BlackQueen = blackQueen;
        BlackKing = blackKing;

        UPawn = uPawn;
        RooksToCastle = castle;
    }

    public void MakePawnMove(bool isWhite, bool isDouble, PieceRank pieceRank, PieceFile pieceFile)
    {
        ulong currentPawnPosition = 1;
        currentPawnPosition <<= (int)pieceFile;
        currentPawnPosition <<= (int)pieceRank * 8;

        if (isWhite)
        {
            if ((currentPawnPosition & WhitePawn) == 0) throw new Exception("No white pawn!");

            WhitePawn ^= currentPawnPosition;
            WhitePawn |= currentPawnPosition << (isDouble ? 16 : 8);
        }
        else
        {
            if ((currentPawnPosition & BlackPawn) == 0) throw new Exception("No black pawn!");
            BlackPawn ^= currentPawnPosition;
            BlackPawn |= currentPawnPosition >> (isDouble ? 16 : 8);
        }
    }

    public override void PrintState()
    {
        var ranks = new Stack<String>();
        for (int i = 0; i < 8; i++)
        {
            var rank = "";
            for (int j = 0; j < 8; j++)
            {
                rank += $" {GetPieceCharByBit((i * 8) + j)}";
            }

            ranks.Push(rank);
        }

        Console.WriteLine("   A B C D E F G H   ");
        for (int i = 8; i > 0; i--)
        {
            Console.WriteLine($"{i} {ranks.Pop()}  {i}");
        }

        Console.WriteLine("   A B C D E F G H   ");
    }

    public override ulong GetWhitePieces()
    {
        return WhitePawn | WhiteKnight | WhiteBishop | WhiteRook | WhiteQueen | WhiteKing;
    }

    public override ulong GetBlackPieces()
    {
        return BlackPawn | BlackKnight | BlackBishop | BlackRook | BlackQueen | BlackKing;
    }

    private char GetPieceCharByBit(int shift)
    {
        if (((WhitePawn >> shift) & 1) == 1) return 'P';
        if (((WhiteKnight >> shift) & 1) == 1) return 'N';
        if (((WhiteBishop >> shift) & 1) == 1) return 'B';
        if (((WhiteRook >> shift) & 1) == 1) return 'R';
        if (((WhiteQueen >> shift) & 1) == 1) return 'Q';
        if (((WhiteKing >> shift) & 1) == 1) return 'K';

        if (((BlackPawn >> shift) & 1) == 1) return 'p';
        if (((BlackKnight >> shift) & 1) == 1) return 'n';
        if (((BlackBishop >> shift) & 1) == 1) return 'b';
        if (((BlackRook >> shift) & 1) == 1) return 'r';
        if (((BlackQueen >> shift) & 1) == 1) return 'q';
        if (((BlackKing >> shift) & 1) == 1) return 'k';

        return 'x';
    }
}