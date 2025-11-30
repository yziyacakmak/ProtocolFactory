using System.Runtime.CompilerServices;
using ProtocolFactory.Core.Contracts;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Core.Math;


public static class Deserialization
{
#if NET9_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
public static unsafe void Deserialize<T, TProtoValue>(T instance, ReadOnlySpan<byte> source)
    where T : class,IProtocol<T>
    where TProtoValue : struct, IProtocolValue<TProtoValue>
        {
                    var protoValues= default(TProtoValue);

        for (var i=0;i< protoValues.FieldCount;i++)
        {
            var startBit= protoValues.StartBits[i];
            var endian= protoValues.Endians[i];
            var byteStartIndex = startBit / 8;
            var fieldBytes = source.Slice(byteStartIndex, protoValues.LengthAsByte[i]);
            var result = 0;

            if (endian == Endianness.Big)
            {
                result=ProtocolPrimitives.ReadInt32BigEndianPtr(fieldBytes);
                result &= protoValues.Masks[i];
                result >>= protoValues.Shifts[i];
            }
            

            instance.Setters[i](instance, (ulong)result);
        }

            

            
        }
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void Deserialize<T, TProtoValue>(ref T instance, byte[] source)
       where T : class, IProtocol<T>
        where TProtoValue : struct, IProtocolValue<TProtoValue>
    {
       
        var protoValues= default(TProtoValue);

        for (int i=0;i< protoValues.FieldCount;i++)
        {
            var startBit= protoValues.StartBits[i];
            var length= protoValues.Lengths[i];
            var endian= protoValues.Endians[i];

            int byteStartIndex = startBit / 8;
            int bitOffset = startBit % 8;
            int totalBits = length;
            int totalBytes = (totalBits + bitOffset + 7) / 8;
            byte[] fieldBytes = new byte[totalBytes];
            Array.Copy(source, byteStartIndex, fieldBytes, 0, totalBytes);
            int shiftStart = bitOffset;
            int result = 0;

            foreach (var byteValue in fieldBytes)
            {
                result |= (byteValue << shiftStart);
                shiftStart += 8;

                if (shiftStart >= 8) shiftStart = 0;
            }

            // Sonucu endian'a göre yeniden düzenle
            if (endian == Endianness.Big)
            {
                // BigEndian için byte'ları tersine çevir
                byte[] resultBytes = BitConverter.GetBytes(result);
                Array.Reverse(resultBytes);
                result = BitConverter.ToInt32(resultBytes, 0);
            }

            instance.Setters[i](instance, (ulong)result);
        }

        Console.WriteLine("Using byte[] overload");

    }
#endif


}

