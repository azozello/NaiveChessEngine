using ChessEngine.Core.State;

namespace ChessEngine.Core.Evaluation;

public interface IEvaluator
{
    /// <summary>
    /// Calculates board state evaluation as a number.
    /// Positive numbers meaning white is better, negative - black.
    /// Values [-1000.0, 1000.0] reserved for mated state.
    /// </summary>
    /// <param name="board"><see cref="ABoard"/>Current state of the board</param>
    /// <returns>Board state evaluation in as a number.</returns>
    public double Evaluate(ABoard board);
}