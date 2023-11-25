using ChessEngine.Core.State;

namespace ChessEngine.Core.Calculation;

public class BitmapPieceCalculus : IPieceCalculus
{
    private const ulong FirstRank = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_11111111;
    private const ulong EightsRank = 0b_11111111_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
    private const ulong AFile = 0b_00000001_00000001_00000001_00000001_00000001_00000001_00000001_00000001;
    private const ulong HFile = 0b_10000000_10000000_10000000_10000000_10000000_10000000_10000000_10000000;

    /// <inheritdoc cref="IPieceCalculus"/>
    public ulong PawnAttack(ulong enemy, ulong piece, bool color, ulong enPassantSquare)
    {
        ThrowIfPawnOnLastRank(piece, color);

        ulong leftTargetSquare = color ? piece << 9 : piece >> 7;
        ulong rightTargetSquare = color ? piece << 7 : piece >> 9;

        ulong enPassant = GetEnPassant(enemy: enemy, piece: piece, enPassantSquare: enPassantSquare);

        if ((piece & AFile) == piece) return (enemy & leftTargetSquare) | enPassant;
        if ((piece & HFile) == piece) return (enemy & rightTargetSquare) | enPassant;

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
        return piece switch
        {
            0b_0000000000000000000000000000000000000000000000000000000000000001 => enemy &
                0b_0000000000000000000000000000000000000000000000100000010000000000,
            0b_0000000000000000000000000000000000000000000000000000000000000010 => enemy &
                0b_0000000000000000000000000000000000000000000001010000100000000000,
            0b_0000000000000000000000000000000000000000000000000000000000000100 => enemy &
                0b_0000000000000000000000000000000000000000000010100001000100000000,
            0b_0000000000000000000000000000000000000000000000000000000000001000 => enemy &
                0b_0000000000000000000000000000000000000000000101000010001000000000,
            0b_0000000000000000000000000000000000000000000000000000000000010000 => enemy &
                0b_0000000000000000000000000000000000000000001010000100010000000000,
            0b_0000000000000000000000000000000000000000000000000000000000100000 => enemy &
                0b_0000000000000000000000000000000000000000010100001000100000000000,
            0b_0000000000000000000000000000000000000000000000000000000001000000 => enemy &
                0b_0000000000000000000000000000000000000000101000000001000000000000,
            0b_0000000000000000000000000000000000000000000000000000000010000000 => enemy &
                0b_0000000000000000000000000000000000000000010000000010000000000000,
            0b_0000000000000000000000000000000000000000000000000000000100000000 => enemy &
                0b_0000000000000000000000000000000000000010000001000000000000000100,
            0b_0000000000000000000000000000000000000000000000000000001000000000 => enemy &
                0b_0000000000000000000000000000000000000101000010000000000000001000,
            0b_0000000000000000000000000000000000000000000000000000010000000000 => enemy &
                0b_0000000000000000000000000000000000001010000100010000000000010001,
            0b_0000000000000000000000000000000000000000000000000000100000000000 => enemy &
                0b_0000000000000000000000000000000000010100001000100000000000100010,
            0b_0000000000000000000000000000000000000000000000000001000000000000 => enemy &
                0b_0000000000000000000000000000000000101000010001000000000001000100,
            0b_0000000000000000000000000000000000000000000000000010000000000000 => enemy &
                0b_0000000000000000000000000000000001010000100010000000000010001000,
            0b_0000000000000000000000000000000000000000000000000100000000000000 => enemy &
                0b_0000000000000000000000000000000010100000000100000000000000010000,
            0b_0000000000000000000000000000000000000000000000001000000000000000 => enemy &
                0b_0000000000000000000000000000000001000000001000000000000000100000,
            0b_0000000000000000000000000000000000000000000000010000000000000000 => enemy &
                0b_0000000000000000000000000000001000000100000000000000010000000010,
            0b_0000000000000000000000000000000000000000000000100000000000000000 => enemy &
                0b_0000000000000000000000000000010100001000000000000000100000000101,
            0b_0000000000000000000000000000000000000000000001000000000000000000 => enemy &
                0b_0000000000000000000000000000101000010001000000000001000100001010,
            0b_0000000000000000000000000000000000000000000010000000000000000000 => enemy &
                0b_0000000000000000000000000001010000100010000000000010001000010100,
            0b_0000000000000000000000000000000000000000000100000000000000000000 => enemy &
                0b_0000000000000000000000000010100001000100000000000100010000101000,
            0b_0000000000000000000000000000000000000000001000000000000000000000 => enemy &
                0b_0000000000000000000000000101000010001000000000001000100001010000,
            0b_0000000000000000000000000000000000000000010000000000000000000000 => enemy &
                0b_0000000000000000000000001010000000010000000000000001000010100000,
            0b_0000000000000000000000000000000000000000100000000000000000000000 => enemy &
                0b_0000000000000000000000000100000000100000000000000010000001000000,
            0b_0000000000000000000000000000000000000001000000000000000000000000 => enemy &
                0b_0000000000000000000000100000010000000000000001000000001000000000,
            0b_0000000000000000000000000000000000000010000000000000000000000000 => enemy &
                0b_0000000000000000000001010000100000000000000010000000010100000000,
            0b_0000000000000000000000000000000000000100000000000000000000000000 => enemy &
                0b_0000000000000000000010100001000100000000000100010000101000000000,
            0b_0000000000000000000000000000000000001000000000000000000000000000 => enemy &
                0b_0000000000000000000101000010001000000000001000100001010000000000,
            0b_0000000000000000000000000000000000010000000000000000000000000000 => enemy &
                0b_0000000000000000001010000100010000000000010001000010100000000000,
            0b_0000000000000000000000000000000000100000000000000000000000000000 => enemy &
                0b_0000000000000000010100001000100000000000100010000101000000000000,
            0b_0000000000000000000000000000000001000000000000000000000000000000 => enemy &
                0b_0000000000000000101000000001000000000000000100001010000000000000,
            0b_0000000000000000000000000000000010000000000000000000000000000000 => enemy &
                0b_0000000000000000010000000010000000000000001000000100000000000000,
            0b_0000000000000000000000000000000100000000000000000000000000000000 => enemy &
                0b_0000000000000010000001000000000000000100000000100000000000000000,
            0b_0000000000000000000000000000001000000000000000000000000000000000 => enemy &
                0b_0000000000000101000010000000000000001000000001010000000000000000,
            0b_0000000000000000000000000000010000000000000000000000000000000000 => enemy &
                0b_0000000000001010000100010000000000010001000010100000000000000000,
            0b_0000000000000000000000000000100000000000000000000000000000000000 => enemy &
                0b_0000000000010100001000100000000000100010000101000000000000000000,
            0b_0000000000000000000000000001000000000000000000000000000000000000 => enemy &
                0b_0000000000101000010001000000000001000100001010000000000000000000,
            0b_0000000000000000000000000010000000000000000000000000000000000000 => enemy &
                0b_0000000001010000100010000000000010001000010100000000000000000000,
            0b_0000000000000000000000000100000000000000000000000000000000000000 => enemy &
                0b_0000000010100000000100000000000000010000101000000000000000000000,
            0b_0000000000000000000000001000000000000000000000000000000000000000 => enemy &
                0b_0000000001000000001000000000000000100000010000000000000000000000,
            0b_0000000000000000000000010000000000000000000000000000000000000000 => enemy &
                0b_0000001000000100000000000000010000000010000000000000000000000000,
            0b_0000000000000000000000100000000000000000000000000000000000000000 => enemy &
                0b_0000010100001000000000000000100000000101000000000000000000000000,
            0b_0000000000000000000001000000000000000000000000000000000000000000 => enemy &
                0b_0000101000010001000000000001000100001010000000000000000000000000,
            0b_0000000000000000000010000000000000000000000000000000000000000000 => enemy &
                0b_0001010000100010000000000010001000010100000000000000000000000000,
            0b_0000000000000000000100000000000000000000000000000000000000000000 => enemy &
                0b_0010100001000100000000000100010000101000000000000000000000000000,
            0b_0000000000000000001000000000000000000000000000000000000000000000 => enemy &
                0b_0101000010001000000000001000100001010000000000000000000000000000,
            0b_0000000000000000010000000000000000000000000000000000000000000000 => enemy &
                0b_1010000000010000000000000001000010100000000000000000000000000000,
            0b_0000000000000000100000000000000000000000000000000000000000000000 => enemy &
                0b_0100000000100000000000000010000001000000000000000000000000000000,
            0b_0000000000000001000000000000000000000000000000000000000000000000 => enemy &
                0b_0000010000000000000001000000001000000000000000000000000000000000,
            0b_0000000000000010000000000000000000000000000000000000000000000000 => enemy &
                0b_0000100000000000000010000000010100000000000000000000000000000000,
            0b_0000000000000100000000000000000000000000000000000000000000000000 => enemy &
                0b_0001000100000000000100010000101000000000000000000000000000000000,
            0b_0000000000001000000000000000000000000000000000000000000000000000 => enemy &
                0b_0010001000000000001000100001010000000000000000000000000000000000,
            0b_0000000000010000000000000000000000000000000000000000000000000000 => enemy &
                0b_0100010000000000010001000010100000000000000000000000000000000000,
            0b_0000000000100000000000000000000000000000000000000000000000000000 => enemy &
                0b_1000100000000000100010000101000000000000000000000000000000000000,
            0b_0000000001000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0001000000000000000100001010000000000000000000000000000000000000,
            0b_0000000010000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0010000000000000001000000100000000000000000000000000000000000000,
            0b_0000000100000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000000000100000000100000000000000000000000000000000000000000,
            0b_0000001000000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000000001000000001010000000000000000000000000000000000000000,
            0b_0000010000000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000000010001000010100000000000000000000000000000000000000000,
            0b_0000100000000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000000100010000101000000000000000000000000000000000000000000,
            0b_0001000000000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000001000100001010000000000000000000000000000000000000000000,
            0b_0010000000000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000010001000010100000000000000000000000000000000000000000000,
            0b_0100000000000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000000010000101000000000000000000000000000000000000000000000,
            0b_1000000000000000000000000000000000000000000000000000000000000000 => enemy &
                0b_0000000000100000010000000000000000000000000000000000000000000000,
            _ => 0
        };
    }

    /// <inheritdoc cref="IPieceCalculus"/>
    public ulong BishopAttack(ulong enemy, ulong own, ulong piece)
    {
        //  <<7     <<9
        //     \   /
        //       B
        //     /   \ 
        // >>9     >>7
        return FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: HFile | EightsRank, left: 9, right: 0)
               | FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: HFile | FirstRank, left: 0, right: 7)
               | FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: AFile | FirstRank, left: 0, right: 9)
               | FollowDiagonal(enemy: enemy, own: own, cursor: piece, bound: AFile | EightsRank, left: 7, right: 0);
    }

    /// <inheritdoc cref="IPieceCalculus"/>
    public ulong RookAttack(ulong own, ulong enemy, ulong piece)
    {
        return SlidingPieceFileAttack(own: own, enemy: enemy, piece: piece)
               | SlidingPieceRankAttack(own: own, enemy: enemy, piece: piece);
    }

    /// <inheritdoc cref="IPieceCalculus"/>
    public ulong QueenAttack(ulong own, ulong enemy, ulong piece)
    {
        return RookAttack(own: own, enemy: enemy, piece: piece)
               | BishopAttack(own: own, enemy: enemy, piece: piece);
    }

    /// TODO: Speed-up! It is only 8 MOPS now! (2 times slower then Queen!)
    /// TODO: Check StockFish implementation or just hard-code (same as Knight).
    /// <inheritdoc cref="IPieceCalculus"/>
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

        if ((piece & EightsRank) == piece)
        {
            skip[0] = 1;
            skip[1] = 1;
            skip[2] = 1;
        }

        if ((piece & HFile) == piece)
        {
            skip[2] = 1;
            skip[3] = 1;
            skip[4] = 1;
        }

        if ((piece & FirstRank) == piece)
        {
            skip[4] = 1;
            skip[5] = 1;
            skip[6] = 1;
        }

        if ((piece & AFile) == piece)
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
        do
        {
            cursor <<= left;
            cursor >>= right;
            if ((cursor & own) == cursor) break;
            if ((cursor & enemy) == cursor)
            {
                attack |= cursor;
                break;
            }
        } while ((cursor & bound) != cursor);

        return attack;
    }

    private void ThrowIfPawnOnLastRank(ulong piece, bool color)
    {
        if ((color && (piece & EightsRank) == piece) || !color && (piece & FirstRank) == piece)
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
        if ((piece & FirstRank) == piece)
        {
            return FollowFile(own: own, enemy: enemy, piece: piece, bound: EightsRank, moveTop: true);
        }

        if ((piece & EightsRank) == piece)
        {
            return FollowFile(own: own, enemy: enemy, piece: piece, bound: FirstRank, moveTop: false);
        }

        return FollowFile(own: own, enemy: enemy, piece: piece, bound: EightsRank, moveTop: true)
               | FollowFile(own: own, enemy: enemy, piece: piece, bound: FirstRank, moveTop: false);
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
        if ((piece & AFile) == piece)
        {
            return GetEnemyPieceAttackedOnALeft(ownPieces: own, enemyPieces: enemy, slidingPiece: piece);
        }

        if ((piece & HFile) == piece)
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