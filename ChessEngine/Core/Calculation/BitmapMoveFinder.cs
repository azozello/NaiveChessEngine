namespace ChessEngine.Core.Calculation;

public class BitmapMoveFinder : IMoveFinder
{
    public ulong PawnMoves(ulong own, ulong enemy, bool color, ulong piece)
    {
        ulong moves = 0;

        if (color && (piece & Constants.Rank8) == 0)
        {
            moves |= ((own | enemy) & (piece << 8)) ^ (piece << 8);
            if ((piece & Constants.Rank2) == piece && moves != 0)
                moves |= ((own | enemy) & (piece << 16)) ^ (piece << 16);
        }
        else if (!color && (piece & Constants.Rank1) == 0)
        {
            moves |= ((own | enemy) & (piece >> 8)) ^ (piece >> 8);
            if ((piece & Constants.Rank7) == piece && moves != 0)
                moves |= ((own | enemy) & (piece >> 16)) ^ (piece >> 16);
        }

        return moves;
    }

    public ulong BishopMoves(ulong own, ulong enemy, ulong piece)
    {
        throw new NotImplementedException();
    }

    public ulong KnightMoves(ulong own, ulong enemy, ulong piece)
    {
        ulong moves = Constants.GetKnightMoves(piece);
        return (moves & (own | enemy)) ^ moves;
    }

    public ulong RookMoves(ulong own, ulong enemy, ulong piece)
    {
        throw new NotImplementedException();
    }

    public ulong QueenMoves(ulong own, ulong enemy, ulong piece)
    {
        throw new NotImplementedException();
    }

    public ulong KingMoves(ulong own, ulong enemy, ulong piece)
    {
        throw new NotImplementedException();
    }
}