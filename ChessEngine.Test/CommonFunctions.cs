namespace ChessEngine.Test;

public class CommonFunctions
{
    

    public static string InvertFen(string fen)
    {
        string invertedFen = "";
        foreach (var c in fen)
        {
            if (c == 'p') invertedFen += "P";
            else if (c == 'n') invertedFen += "N";
            else if (c == 'b') invertedFen += "B";
            else if (c == 'r') invertedFen += "R";
            else if (c == 'q') invertedFen += "Q";
            else if (c == 'k') invertedFen += "K";

            else if (c == 'P') invertedFen += "p";
            else if (c == 'N') invertedFen += "n";
            else if (c == 'B') invertedFen += "b";
            else if (c == 'R') invertedFen += "r";
            else if (c == 'Q') invertedFen += "q";
            else if (c == 'K') invertedFen += "k";

            else invertedFen += c;
        }

        return invertedFen;
    }
}