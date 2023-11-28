using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ChessEngine.Core.Calculation;

public class Program
{
    private readonly ulong[][] _cases = new ulong[1_000_000][];
    private readonly BitmapMoveFinder _calculus = new();

    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Program>();
    }

    [GlobalSetup]
    public void SetUp()
    {
        var random = new Random();

        for (int i = 0; i < 1_000_000; i++)
        {
            var own = (ulong)random.NextInt64();
            var enemy = (ulong)random.NextInt64();
            var piece = (ulong)random.NextInt64();

            _cases[i] = new[] { own, enemy, piece };
        }
    }

    [Benchmark]
    public void Benchmark()
    {
        for (int i = 0; i < 1_000_000; i++)
        {
            _calculus.RookMoves(_cases[i][0], _cases[i][1], _cases[i][2]);
        }
    }
}