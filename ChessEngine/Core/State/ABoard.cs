namespace ChessEngine.Core.State;

public abstract class ABoard
{
    public ulong WhitePawn { get; protected set; }
    public ulong WhiteKnight { get; protected set; }
    public ulong WhiteBishop { get; protected set; }
    public ulong WhiteRook { get; protected set; }
    public ulong WhiteQueen { get; protected set; }
    public ulong WhiteKing { get; protected set; }

    public ulong BlackPawn { get; protected set; }
    public ulong BlackKnight { get; protected set; }
    public ulong BlackBishop { get; protected set; }
    public ulong BlackRook { get; protected set; }
    public ulong BlackQueen { get; protected set; }
    public ulong BlackKing { get; protected set; }

    public ulong UPawn { get; set; }
    public ulong RooksToCastle { get; protected set; }

    public abstract void PrintState();

    public abstract ulong GetWhitePieces();

    public abstract ulong GetBlackPieces();
}