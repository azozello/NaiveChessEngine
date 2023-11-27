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

    public ulong KnightMoves(ulong own, ulong enemy, ulong piece)
    {
        ulong moves = Constants.GetKnightMoves(piece);
        return (moves & (own | enemy)) ^ moves;
    }

    public ulong BishopMoves(ulong own, ulong enemy, ulong piece)
    {
        //  <<7     <<9
        //     \   /
        //       B
        //     /   \ 
        // >>9     >>7
        return FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: Constants.HFile | Constants.Rank8,
                   left: 9, right: 0)
               | FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: Constants.HFile | Constants.Rank1,
                   left: 0, right: 7)
               | FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: Constants.AFile | Constants.Rank1,
                   left: 0, right: 9)
               | FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: Constants.AFile | Constants.Rank8,
                   left: 7, right: 0);
    }

    public ulong RookMoves(ulong own, ulong enemy, ulong piece)
    {
        return FollowFile(own, enemy, piece, Constants.Rank8, true)
               | FollowFile(own, enemy, piece, Constants.Rank1, false)
               | FollowRank(own, enemy, piece, Constants.HFile, true)
               | FollowRank(own, enemy, piece, Constants.AFile, false);
    }

    public ulong QueenMoves(ulong own, ulong enemy, ulong piece)
    {
        return RookMoves(own, enemy, piece) | BishopMoves(own, enemy, piece);
    }

    public ulong KingMoves(ulong own, ulong enemy, ulong piece)
    {
        ulong moves = Constants.GetKingMoves(piece);
        return (moves & (own | enemy)) ^ moves;
    }

    private ulong FollowFile(ulong own, ulong enemy, ulong cursor, ulong bound, bool moveTop)
    {
        ulong moves = 0;

        while ((cursor & bound) != cursor)
        {
            // Slightly (1-2 MOPS) faster than ternary.
            if (moveTop) cursor <<= 8;
            else cursor >>= 8;

            if ((cursor & (own | enemy)) == cursor) break;
            moves |= cursor;
        }

        return moves;
    }

    private ulong FollowRank(ulong own, ulong enemy, ulong cursor, ulong bound, bool moveRight)
    {
        ulong moves = 0;

        while ((cursor & bound) != cursor)
        {
            // Slightly (1-2 MOPS) faster than ternary.
            if (moveRight) cursor <<= 1;
            else cursor >>= 1;

            if ((cursor & (own | enemy)) == cursor) break;
            moves |= cursor;
        }

        return moves;
    }

    private ulong FollowDiagonal(ulong enemy, ulong own, ulong cursor, ulong bound, byte left, byte right)
    {
        ulong moves = 0;
        while ((cursor & bound) != cursor)
        {
            cursor <<= left;
            cursor >>= right;
            if ((cursor & (own | enemy)) == cursor) break;
            moves |= cursor;
        }

        return moves;
    }
}