using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ProtocolFactory.Core.Contracts;

namespace ProtocolFactory.Core.Math;

public static class ProtocolPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T ReadBigEndian<T>(ReadOnlySpan<byte> source)
        where T : struct
    {
        var bufferSize=sizeof(T);
        var sourceLength=source.Length;
        if (sourceLength < bufferSize)
        {
            Span<byte> buffer = stackalloc byte[bufferSize];
            source.CopyTo(buffer.Slice(bufferSize-source.Length, sourceLength));
            return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(buffer));

        }
        return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(source));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt32BigEndian(ReadOnlySpan<byte> source)
    {
        return BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(ReadBigEndian<int>(source)) : ReadBigEndian<int>(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt32BigEndian(ReadOnlySpan<byte> source)
    {
        return BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(ReadBigEndian<uint>(source)) : ReadBigEndian<uint>(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadUInt64BigEndian(ReadOnlySpan<byte> source)
    {
        return BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(ReadBigEndian<ulong>(source)) : ReadBigEndian<ulong>(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadInt64BigEndian(ReadOnlySpan<byte> source)
    {
        return BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(ReadBigEndian<long>(source)) : ReadBigEndian<long>(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T ReadBigEndianPtr<T>(ReadOnlySpan<byte> source)
        where T : struct
    {
        var value=Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(source));
        return value;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt32BigEndianPtr(ReadOnlySpan<byte> source)
    {
        const int bufferSize = sizeof(int);
        var sourceLength=source.Length;
        var retValue=Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(source));
        if (BitConverter.IsLittleEndian)
        {
            retValue = BinaryPrimitives.ReverseEndianness(retValue);
            return retValue >>((bufferSize - sourceLength)*8);
        }
        return retValue;
    }



}