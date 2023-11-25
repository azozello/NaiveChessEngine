using ChessEngine.Core.Calculation;
using ChessEngine.Core.State;

namespace ChessEngine.Core.Evaluation;

public class NaiveEvaluator : IEvaluator
{
    private readonly IPieceCalculus _calculus;

    public NaiveEvaluator(IPieceCalculus calculus)
    {
        _calculus = calculus;
    }

    /// <inheritdoc cref="IEvaluator"/>
    public double Evaluate(ABoard board)
    {
        return -10.0;
    }

    private bool IsKingMated(ABoard board, bool isWhite)
    {
        var isMated = false;

        return isMated;
    }
}