namespace ChessEngine.Core.Calculation;

public static class Constants
{
    public static readonly ulong Rank1 = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_11111111;
    public static readonly ulong Rank2 = 0b_00000000_00000000_00000000_00000000_00000000_00000000_11111111_00000000;
    public static readonly ulong Rank3 = 0b_00000000_00000000_00000000_00000000_00000000_11111111_00000000_00000000;
    public static readonly ulong Rank4 = 0b_00000000_00000000_00000000_00000000_11111111_00000000_00000000_00000000;
    public static readonly ulong Rank5 = 0b_00000000_00000000_00000000_11111111_00000000_00000000_00000000_00000000;
    public static readonly ulong Rank6 = 0b_00000000_00000000_11111111_00000000_00000000_00000000_00000000_00000000;
    public static readonly ulong Rank7 = 0b_00000000_11111111_00000000_00000000_00000000_00000000_00000000_00000000;
    public static readonly ulong Rank8 = 0b_11111111_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

    public static readonly ulong AFile = 0b_00000001_00000001_00000001_00000001_00000001_00000001_00000001_00000001;
    public static readonly ulong HFile = 0b_10000000_10000000_10000000_10000000_10000000_10000000_10000000_10000000;

    public static ulong GetKnightMoves(ulong piece)
    {
        return piece switch
        {
            0b_0000000000000000000000000000000000000000000000000000000000000001 =>
                0b_0000000000000000000000000000000000000000000000100000010000000000,
            0b_0000000000000000000000000000000000000000000000000000000000000010 =>
                0b_0000000000000000000000000000000000000000000001010000100000000000,
            0b_0000000000000000000000000000000000000000000000000000000000000100 =>
                0b_0000000000000000000000000000000000000000000010100001000100000000,
            0b_0000000000000000000000000000000000000000000000000000000000001000 =>
                0b_0000000000000000000000000000000000000000000101000010001000000000,
            0b_0000000000000000000000000000000000000000000000000000000000010000 =>
                0b_0000000000000000000000000000000000000000001010000100010000000000,
            0b_0000000000000000000000000000000000000000000000000000000000100000 =>
                0b_0000000000000000000000000000000000000000010100001000100000000000,
            0b_0000000000000000000000000000000000000000000000000000000001000000 =>
                0b_0000000000000000000000000000000000000000101000000001000000000000,
            0b_0000000000000000000000000000000000000000000000000000000010000000 =>
                0b_0000000000000000000000000000000000000000010000000010000000000000,
            0b_0000000000000000000000000000000000000000000000000000000100000000 =>
                0b_0000000000000000000000000000000000000010000001000000000000000100,
            0b_0000000000000000000000000000000000000000000000000000001000000000 =>
                0b_0000000000000000000000000000000000000101000010000000000000001000,
            0b_0000000000000000000000000000000000000000000000000000010000000000 =>
                0b_0000000000000000000000000000000000001010000100010000000000010001,
            0b_0000000000000000000000000000000000000000000000000000100000000000 =>
                0b_0000000000000000000000000000000000010100001000100000000000100010,
            0b_0000000000000000000000000000000000000000000000000001000000000000 =>
                0b_0000000000000000000000000000000000101000010001000000000001000100,
            0b_0000000000000000000000000000000000000000000000000010000000000000 =>
                0b_0000000000000000000000000000000001010000100010000000000010001000,
            0b_0000000000000000000000000000000000000000000000000100000000000000 =>
                0b_0000000000000000000000000000000010100000000100000000000000010000,
            0b_0000000000000000000000000000000000000000000000001000000000000000 =>
                0b_0000000000000000000000000000000001000000001000000000000000100000,
            0b_0000000000000000000000000000000000000000000000010000000000000000 =>
                0b_0000000000000000000000000000001000000100000000000000010000000010,
            0b_0000000000000000000000000000000000000000000000100000000000000000 =>
                0b_0000000000000000000000000000010100001000000000000000100000000101,
            0b_0000000000000000000000000000000000000000000001000000000000000000 =>
                0b_0000000000000000000000000000101000010001000000000001000100001010,
            0b_0000000000000000000000000000000000000000000010000000000000000000 =>
                0b_0000000000000000000000000001010000100010000000000010001000010100,
            0b_0000000000000000000000000000000000000000000100000000000000000000 =>
                0b_0000000000000000000000000010100001000100000000000100010000101000,
            0b_0000000000000000000000000000000000000000001000000000000000000000 =>
                0b_0000000000000000000000000101000010001000000000001000100001010000,
            0b_0000000000000000000000000000000000000000010000000000000000000000 =>
                0b_0000000000000000000000001010000000010000000000000001000010100000,
            0b_0000000000000000000000000000000000000000100000000000000000000000 =>
                0b_0000000000000000000000000100000000100000000000000010000001000000,
            0b_0000000000000000000000000000000000000001000000000000000000000000 =>
                0b_0000000000000000000000100000010000000000000001000000001000000000,
            0b_0000000000000000000000000000000000000010000000000000000000000000 =>
                0b_0000000000000000000001010000100000000000000010000000010100000000,
            0b_0000000000000000000000000000000000000100000000000000000000000000 =>
                0b_0000000000000000000010100001000100000000000100010000101000000000,
            0b_0000000000000000000000000000000000001000000000000000000000000000 =>
                0b_0000000000000000000101000010001000000000001000100001010000000000,
            0b_0000000000000000000000000000000000010000000000000000000000000000 =>
                0b_0000000000000000001010000100010000000000010001000010100000000000,
            0b_0000000000000000000000000000000000100000000000000000000000000000 =>
                0b_0000000000000000010100001000100000000000100010000101000000000000,
            0b_0000000000000000000000000000000001000000000000000000000000000000 =>
                0b_0000000000000000101000000001000000000000000100001010000000000000,
            0b_0000000000000000000000000000000010000000000000000000000000000000 =>
                0b_0000000000000000010000000010000000000000001000000100000000000000,
            0b_0000000000000000000000000000000100000000000000000000000000000000 =>
                0b_0000000000000010000001000000000000000100000000100000000000000000,
            0b_0000000000000000000000000000001000000000000000000000000000000000 =>
                0b_0000000000000101000010000000000000001000000001010000000000000000,
            0b_0000000000000000000000000000010000000000000000000000000000000000 =>
                0b_0000000000001010000100010000000000010001000010100000000000000000,
            0b_0000000000000000000000000000100000000000000000000000000000000000 =>
                0b_0000000000010100001000100000000000100010000101000000000000000000,
            0b_0000000000000000000000000001000000000000000000000000000000000000 =>
                0b_0000000000101000010001000000000001000100001010000000000000000000,
            0b_0000000000000000000000000010000000000000000000000000000000000000 =>
                0b_0000000001010000100010000000000010001000010100000000000000000000,
            0b_0000000000000000000000000100000000000000000000000000000000000000 =>
                0b_0000000010100000000100000000000000010000101000000000000000000000,
            0b_0000000000000000000000001000000000000000000000000000000000000000 =>
                0b_0000000001000000001000000000000000100000010000000000000000000000,
            0b_0000000000000000000000010000000000000000000000000000000000000000 =>
                0b_0000001000000100000000000000010000000010000000000000000000000000,
            0b_0000000000000000000000100000000000000000000000000000000000000000 =>
                0b_0000010100001000000000000000100000000101000000000000000000000000,
            0b_0000000000000000000001000000000000000000000000000000000000000000 =>
                0b_0000101000010001000000000001000100001010000000000000000000000000,
            0b_0000000000000000000010000000000000000000000000000000000000000000 =>
                0b_0001010000100010000000000010001000010100000000000000000000000000,
            0b_0000000000000000000100000000000000000000000000000000000000000000 =>
                0b_0010100001000100000000000100010000101000000000000000000000000000,
            0b_0000000000000000001000000000000000000000000000000000000000000000 =>
                0b_0101000010001000000000001000100001010000000000000000000000000000,
            0b_0000000000000000010000000000000000000000000000000000000000000000 =>
                0b_1010000000010000000000000001000010100000000000000000000000000000,
            0b_0000000000000000100000000000000000000000000000000000000000000000 =>
                0b_0100000000100000000000000010000001000000000000000000000000000000,
            0b_0000000000000001000000000000000000000000000000000000000000000000 =>
                0b_0000010000000000000001000000001000000000000000000000000000000000,
            0b_0000000000000010000000000000000000000000000000000000000000000000 =>
                0b_0000100000000000000010000000010100000000000000000000000000000000,
            0b_0000000000000100000000000000000000000000000000000000000000000000 =>
                0b_0001000100000000000100010000101000000000000000000000000000000000,
            0b_0000000000001000000000000000000000000000000000000000000000000000 =>
                0b_0010001000000000001000100001010000000000000000000000000000000000,
            0b_0000000000010000000000000000000000000000000000000000000000000000 =>
                0b_0100010000000000010001000010100000000000000000000000000000000000,
            0b_0000000000100000000000000000000000000000000000000000000000000000 =>
                0b_1000100000000000100010000101000000000000000000000000000000000000,
            0b_0000000001000000000000000000000000000000000000000000000000000000 =>
                0b_0001000000000000000100001010000000000000000000000000000000000000,
            0b_0000000010000000000000000000000000000000000000000000000000000000 =>
                0b_0010000000000000001000000100000000000000000000000000000000000000,
            0b_0000000100000000000000000000000000000000000000000000000000000000 =>
                0b_0000000000000100000000100000000000000000000000000000000000000000,
            0b_0000001000000000000000000000000000000000000000000000000000000000 =>
                0b_0000000000001000000001010000000000000000000000000000000000000000,
            0b_0000010000000000000000000000000000000000000000000000000000000000 =>
                0b_0000000000010001000010100000000000000000000000000000000000000000,
            0b_0000100000000000000000000000000000000000000000000000000000000000 =>
                0b_0000000000100010000101000000000000000000000000000000000000000000,
            0b_0001000000000000000000000000000000000000000000000000000000000000 =>
                0b_0000000001000100001010000000000000000000000000000000000000000000,
            0b_0010000000000000000000000000000000000000000000000000000000000000 =>
                0b_0000000010001000010100000000000000000000000000000000000000000000,
            0b_0100000000000000000000000000000000000000000000000000000000000000 =>
                0b_0000000000010000101000000000000000000000000000000000000000000000,
            0b_1000000000000000000000000000000000000000000000000000000000000000 =>
                0b_0000000000100000010000000000000000000000000000000000000000000000,
            _ => 0
        };
    }
}