using ChessEngine.Core.State;

namespace ChessEngine.Core.Calculation;

public class BitmapPieceCalculus : IPieceCalculus
{
    private const ulong FirstRank = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_11111111;
    private const ulong EightsRank = 0b_11111111_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
    private const ulong AFile = 0b_00000001_00000001_00000001_00000001_00000001_00000001_00000001_00000001;
    private const ulong HFile = 0b_10000000_10000000_10000000_10000000_10000000_10000000_10000000_10000000;

    public ulong PiecesAttackedByRooks(ABoard board, bool color)
    {
        ulong piecesUnderAttack = 0;
        ulong rank = FirstRank;
        for (int i = 0; i < 8; i++)
        {
            ulong file = AFile;
            for (int j = 0; j < 8; j++)
            {
                if ((board.WhiteRook & file & rank) != 0)
                {
                    piecesUnderAttack |= file & rank;
                }
                else if ((board.BlackRook & file & rank) != 0)
                {
                    piecesUnderAttack |= file & rank;
                }

                file <<= 8 * i;
            }

            rank <<= 8 * i;
        }

        return piecesUnderAttack;
    }

    public ulong KnightAttack(ulong enemy, ulong piece, bool color)
    {
        return 0;
    }

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

    public ulong SlidingPieceRankAttack(ulong own, ulong enemy, ulong piece)
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