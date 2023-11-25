namespace ChessEngine.Utils;

public delegate void DoLog(string data);

public class ChessLogger
{
    private const string FileNames = "   A B C D E F G H   ";
    private readonly DoLog _log;

    public ChessLogger(DoLog log)
    {
        _log = log;
    }

    public void LogBitmap(ulong bitmap)
    {
        char[] representation = new char [64];
        for (int i = 63; i >= 0; i--)
        {
            representation[i] = (bitmap & 1) == 1 ? '1' : '0';
            bitmap >>= 1;
        }

        _log(new string(representation));
    }

    public void LogAsBoard(ulong bitmap, bool verbose = false)
    {
        var ranks = new Stack<String>();
        for (int i = 0; i < 8; i++)
        {
            var rank = "";
            for (int j = 0; j < 8; j++)
            {
                rank += $"{GetBit(bitmap, (i * 8) + j)} ";
            }

            ranks.Push(rank);
        }

        if (verbose) _log(FileNames);
        for (int i = 8; i > 0; i--)
        {
            _log(verbose ? $"{i}  {ranks.Pop()} {i}" : ranks.Pop().Trim());
        }

        if (verbose) _log(FileNames);
    }

    private char GetBit(ulong bitmap, int shift)
    {
        return ((bitmap >> shift) & 1) == 1 ? '1' : '0';
    }
}