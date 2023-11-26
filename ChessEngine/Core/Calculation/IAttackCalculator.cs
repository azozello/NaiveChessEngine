using ChessEngine.Core.State;

namespace ChessEngine.Core.Calculation;

public interface IAttackCalculator
{
    ulong PawnAttack(ulong enemy, ulong piece, bool color, ulong enPassantSquare);
    ulong KnightAttack(ulong enemy, ulong piece);
    ulong BishopAttack(ulong own, ulong enemy, ulong piece);
    ulong RookAttack(ulong own, ulong enemy, ulong piece);
    ulong QueenAttack(ulong own, ulong enemy, ulong piece);
    ulong KingAttack(ulong enemy, ulong piece);
}