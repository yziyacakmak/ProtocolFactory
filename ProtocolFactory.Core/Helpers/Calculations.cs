namespace ProtocolFactory.Core.Helpers;


public static class Calculations
{
    internal static int GetBigEndianLsbIndex(int msbBitIndex, int length)
    {
        var bitIndex = msbBitIndex;
        for (var i = 0; i < length-1; i++)
        {
            if ((bitIndex & 7) == 0)
            {
                bitIndex += 15;
            }
            else
            {
                --bitIndex;
            }
        }
        return bitIndex;
    }


    internal static int GetLittleEndianMsbIndex(int lsbBitIndex, int length)
    {
        return lsbBitIndex + length;
    }

    private static byte GetBitMaskUpTo(int bitIndex)
    {
        return (byte)((1 << (bitIndex + 1)) - 1);
    }
    internal static byte GetMsbByteMask(int msbBitInByte)
    {
        return GetBitMaskUpTo(msbBitInByte);
    }

    internal static byte GetLsbByteMask(int lsbBitInByte)
    {
        return (byte)~((1 << (lsbBitInByte)) - 1);
    }

    internal static ulong GetByteCountMask(int byteCount)
    {
        var bitCount = byteCount * 8;
        if (bitCount < 0 || bitCount > 64)
            throw new ArgumentOutOfRangeException(nameof(bitCount), "0 ile 64 arasında olmalı");

        return bitCount == 64 ? ulong.MaxValue : (1UL << bitCount) - 1;
    }
}