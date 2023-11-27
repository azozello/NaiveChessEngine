using ChessEngine.Core.State;

namespace ChessEngine.Core.Calculation;

public class BitmapAttackCalculator : IAttackCalculator
{
    /// <inheritdoc cref="IAttackCalculator"/>
    public ulong PawnAttack(ulong enemy, ulong piece, bool color, ulong enPassantSquare)
    {
        ThrowIfPawnOnLastRank(piece, color);

        ulong leftTargetSquare = color ? piece << 9 : piece >> 7;
        ulong rightTargetSquare = color ? piece << 7 : piece >> 9;

        ulong enPassant = GetEnPassant(enemy: enemy, piece: piece, enPassantSquare: enPassantSquare);

        if ((piece & Constants.AFile) == piece) return (enemy & leftTargetSquare) | enPassant;
        if ((piece & Constants.HFile) == piece) return (enemy & rightTargetSquare) | enPassant;

        return (enemy & leftTargetSquare) | (enemy & rightTargetSquare) | enPassant;
    }

    /// <summary>
    /// Knight in chess has complicated attack pattern.
    /// Algorithmic implementation would be both convoluted and slow.
    /// fortunately, it is possible to hardcode attack pattern for all 64 cells.
    /// </summary>
    /// <param name="enemy">All enemy pieces at one bitmap</param>
    /// <param name="piece">Knight's position</param>
    /// <returns></returns>
    public ulong KnightAttack(ulong enemy, ulong piece)
    {
        return enemy & Constants.GetKnightMoves(piece);
    }

    /// <inheritdoc cref="IAttackCalculator"/>
    public ulong BishopAttack(ulong enemy, ulong own, ulong piece)
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

    /// <inheritdoc cref="IAttackCalculator"/>
    public ulong RookAttack(ulong own, ulong enemy, ulong piece)
    {
        return SlidingPieceFileAttack(own: own, enemy: enemy, piece: piece)
               | SlidingPieceRankAttack(own: own, enemy: enemy, piece: piece);
    }

    /// <inheritdoc cref="IAttackCalculator"/>
    public ulong QueenAttack(ulong own, ulong enemy, ulong piece)
    {
        return RookAttack(own: own, enemy: enemy, piece: piece)
               | BishopAttack(own: own, enemy: enemy, piece: piece);
    }

    /// TODO: Speed-up! It is only 8 MOPS now! (2 times slower then Queen!)
    /// TODO: Check StockFish implementation or just hard-code (same as Knight).
    /// <inheritdoc cref="IAttackCalculator"/>
    public ulong KingAttack(ulong enemy, ulong piece)
    {
        ulong mask = 0;

        // North | -----------------
        //  East |             -----------------
        // South |                         ----------------
        //  West | ----                                ----------
        //         0. 7; 1. 8; 2. 9; 3. 1; 4.-7; 5.-8; 6.-9; 7.-1
        int[] shifts = new[] { 7, 8, 9, 1, 7, 8, 9, 1 };
        int[] skip = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        if ((piece & Constants.Rank8) == piece)
        {
            skip[0] = 1;
            skip[1] = 1;
            skip[2] = 1;
        }

        if ((piece & Constants.HFile) == piece)
        {
            skip[2] = 1;
            skip[3] = 1;
            skip[4] = 1;
        }

        if ((piece & Constants.Rank1) == piece)
        {
            skip[4] = 1;
            skip[5] = 1;
            skip[6] = 1;
        }

        if ((piece & Constants.AFile) == piece)
        {
            skip[6] = 1;
            skip[7] = 1;
            skip[0] = 1;
        }

        for (int i = 0; i < 8; i++)
        {
            if (skip[i] == 0)
            {
                if (i <= 3) mask |= piece << shifts[i];
                else mask |= piece >> shifts[i];
            }
        }

        return enemy & mask;
    }

    private ulong FollowDiagonal(ulong enemy, ulong own, ulong cursor, ulong bound, byte left, byte right)
    {
        ulong attack = 0;
        while ((cursor & bound) != cursor)
        {
            cursor <<= left;
            cursor >>= right;
            if ((cursor & own) == cursor) break;
            if ((cursor & enemy) == cursor)
            {
                attack |= cursor;
                break;
            }
        }

        return attack;
    }

    private void ThrowIfPawnOnLastRank(ulong piece, bool color)
    {
        if ((color && (piece & Constants.Rank8) == piece) || !color && (piece & Constants.Rank1) == piece)
        {
            throw new ArgumentException(
                color ? "White pawn can't exist on 8th rank!" : "Black pawn can't exist on 1th rank!"
            );
        }
    }

    private ulong GetEnPassant(ulong enemy, ulong piece, ulong enPassantSquare)
    {
        return (enemy & enPassantSquare) == enPassantSquare
            ? ((piece << 1) & enPassantSquare) | ((piece >> 1) & enPassantSquare)
            : 0;
    }

    private ulong SlidingPieceFileAttack(ulong own, ulong enemy, ulong piece)
    {
        if ((piece & Constants.Rank1) == piece)
        {
            return FollowFile(own: own, enemy: enemy, piece: piece, bound: Constants.Rank8, moveTop: true);
        }

        if ((piece & Constants.Rank8) == piece)
        {
            return FollowFile(own: own, enemy: enemy, piece: piece, bound: Constants.Rank1, moveTop: false);
        }

        return FollowFile(own: own, enemy: enemy, piece: piece, bound: Constants.Rank8, moveTop: true)
               | FollowFile(own: own, enemy: enemy, piece: piece, bound: Constants.Rank1, moveTop: false);
    }

    private ulong FollowFile(ulong own, ulong enemy, ulong piece, ulong bound, bool moveTop)
    {
        ulong cursor = piece;
        ulong attack = 0;

        do
        {
            // Slightly (1-2 MOPS) faster than ternary.
            if (moveTop) cursor <<= 8;
            else cursor >>= 8;

            if ((cursor & own) == cursor) break;
            if ((cursor & enemy) == cursor)
            {
                attack |= cursor;
                break;
            }
        } while ((cursor & bound) != cursor);

        return attack;
    }

    // TODO: Unify with file attack/ bishop attack.
    private ulong SlidingPieceRankAttack(ulong own, ulong enemy, ulong piece)
    {
        if ((piece & Constants.AFile) == piece)
        {
            return GetEnemyPieceAttackedOnALeft(ownPieces: own, enemyPieces: enemy, slidingPiece: piece);
        }

        if ((piece & Constants.HFile) == piece)
        {
            return GetEnemyPieceAttackedOnARight(ownPieces: own, enemyPieces: enemy, slidingPiece: piece);
        }

        return GetEnemyPieceAttackedOnALeft(ownPieces: own, enemyPieces: enemy, slidingPiece: piece) |
               GetEnemyPieceAttackedOnARight(ownPieces: own, enemyPieces: enemy, slidingPiece: piece);
    }

    private ulong GetEnemyPieceAttackedOnALeft(ulong ownPieces, ulong enemyPieces, ulong slidingPiece)
    {
        short shift = 0;
        ownPieces ^= slidingPiece;

        while ((ownPieces & slidingPiece) == 0 && shift < 8)
        {
            if ((slidingPiece & enemyPieces) == slidingPiece)
            {
                return slidingPiece << shift;
            }

            shift++;
            ownPieces >>= 1;
            enemyPieces >>= 1;
        }

        return 0;
    }

    private ulong GetEnemyPieceAttackedOnARight(ulong ownPieces, ulong enemyPieces, ulong slidingPiece)
    {
        short shift = 0;
        ownPieces ^= slidingPiece;

        while ((ownPieces & slidingPiece) == 0 && shift >= 0)
        {
            if ((slidingPiece & enemyPieces) == slidingPiece)
            {
                return slidingPiece >> shift;
            }

            shift++;
            ownPieces <<= 1;
            enemyPieces <<= 1;
        }

        return 0;
    }
}