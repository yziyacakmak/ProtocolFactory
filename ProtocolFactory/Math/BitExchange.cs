namespace ProtocolFactory.Core.Math;

public class BitExchange
{
    public static int MsbToLsbBigEndian(int msbBit, int length)
    {
        var startBitInByte = msbBit % 8;
        var startByte = msbBit / 8;
        var bitsInFirstByte = startBitInByte + 1;
    
        if (length <= bitsInFirstByte)
        {
            return msbBit - (length - 1);
        }
        var remainingBits = length - bitsInFirstByte;
        var fullBytes = (remainingBits - 1) / 8;
        var bitsInLastByte = ((remainingBits - 1) % 8) + 1;
        var targetByte = startByte + fullBytes + 1;
        var lsbBitInByte = 7 - (bitsInLastByte - 1);
        return targetByte * 8 + lsbBitInByte;
    }
    
    public static int MaskCalculation(int msbBit,int length)
    {
        var lsbBit = MsbToLsbBigEndian(msbBit,length);
        var startByteIndex = msbBit / 8;
        var endByteIndex = lsbBit / 8;
        
        var maskLength = endByteIndex - startByteIndex + 1;
        var mask =(1<<maskLength*8) - 1;
        
        var startBitInByte= msbBit % 8;
        var startMask=0xFF>>(7-startBitInByte);
        startMask <<= ((maskLength - 1) * 8);
        
        var lsbBitInByte = lsbBit % 8;
        var endMask = 0xFF - ((1 << lsbBitInByte)-1);  

        mask &= ~(0xFF << ((maskLength - 1) * 8));
        mask &= ~0xFF;
        mask |= startMask;
        if (startByteIndex != lsbBit/8)
        {
            mask |= endMask;   
        }

        return mask;
    }

    public static int ShiftAmount(int msbBit, int length)
    {
        var lsbBit = MsbToLsbBigEndian(msbBit,length);
        return  lsbBit % 8;
    }

    public static int LsbToMsbLittleEndian(int lsbBit, int length)
    {
        return lsbBit + length - 1;
    }
    public static int MaskCalculationLittleEndian(int lsbBit,int length)
    {
        var startByteIndex = lsbBit / 8;
        var mask = ((1 << length) - 1) << lsbBit;
        mask >>= (startByteIndex * 8);
        return mask;
    }
    public static int ShiftAmountLittleEndian(int lsbBit)
    {
        return  lsbBit % 8;
    }
    
}