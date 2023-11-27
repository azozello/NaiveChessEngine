namespace ChessEngine.Core.Calculation;

public interface IMoveFinder
{
    ulong PawnMoves(ulong own, ulong enemy, bool color, ulong piece);

    ulong BishopMoves(ulong own, ulong enemy, ulong piece);

    ulong KnightMoves(ulong own, ulong enemy, ulong piece);

    ulong RookMoves(ulong own, ulong enemy, ulong piece);

    ulong QueenMoves(ulong own, ulong enemy, ulong piece);

    ulong KingMoves(ulong own, ulong enemy, ulong piece);
}