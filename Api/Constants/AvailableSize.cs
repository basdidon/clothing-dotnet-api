namespace Api.Constants
{
    [Flags]
    public enum AvaliableSizes
    {
        None = 0,
        XS = 1 << 0,  // 1
        S = 1 << 1,  // 2
        M = 1 << 2,  // 4
        L = 1 << 3,  // 8
        XL = 1 << 4,   // 16
        XXL = 1 << 5,  // 32
        XXXL = 1 << 6, // 64
        All = XS | S | M | L | XL | XXL | XXXL
    }
}
