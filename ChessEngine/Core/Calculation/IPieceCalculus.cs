using ChessEngine.Core.State;

namespace ChessEngine.Core.Calculation;

public interface IPieceCalculus
{
    ulong PiecesAttackedByRooks(ABoard board, bool color);
}