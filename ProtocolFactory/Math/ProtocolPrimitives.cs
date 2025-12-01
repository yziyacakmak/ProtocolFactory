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
    public static int ReadBigEndianInt32Unsafe(ReadOnlySpan<byte> source)
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadBigEndianInt64Unsafe(ReadOnlySpan<byte> source)
    {
        const int bufferSize = sizeof(long);
        var sourceLength=source.Length;
        var retValue=Unsafe.ReadUnaligned<long>(ref MemoryMarshal.GetReference(source));
        if (BitConverter.IsLittleEndian)
        {
            retValue = BinaryPrimitives.ReverseEndianness(retValue);
            return retValue >>((bufferSize - sourceLength)*8);
        }
        return retValue;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadBigEndianUInt32Unsafe(ReadOnlySpan<byte> source)
    {
        const int bufferSize = sizeof(uint);
        var sourceLength=source.Length;
        var retValue=Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(source));
        if (BitConverter.IsLittleEndian)
        {
            retValue = BinaryPrimitives.ReverseEndianness(retValue);
            return retValue >>((bufferSize - sourceLength)*8);
        }
        return retValue;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadBigEndianUInt64Unsafe(ReadOnlySpan<byte> source)
    {
        const int bufferSize = sizeof(ulong);
        var sourceLength=source.Length;
        var retValue=Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(source));
        if (BitConverter.IsLittleEndian)
        {
            retValue = BinaryPrimitives.ReverseEndianness(retValue);
            return retValue >>((bufferSize - sourceLength)*8);
        }
        return retValue;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadBigEndianUInt16Unsafe(ReadOnlySpan<byte> source)
    {
        const int bufferSize = sizeof(ushort);
        var sourceLength=source.Length;
        var retValue=Unsafe.ReadUnaligned<ushort>(ref MemoryMarshal.GetReference(source));
        if (BitConverter.IsLittleEndian)
        {
            retValue = BinaryPrimitives.ReverseEndianness(retValue);
            return (ushort)(retValue >>((bufferSize - sourceLength)*8));
        }
        return retValue;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ReadBigEndianInt16Unsafe(ReadOnlySpan<byte> source)
    {
        const int bufferSize = sizeof(short);
        var sourceLength=source.Length;
        var retValue=Unsafe.ReadUnaligned<short>(ref MemoryMarshal.GetReference(source));
        if (BitConverter.IsLittleEndian)
        {
            retValue = BinaryPrimitives.ReverseEndianness(retValue);
            return (short)(retValue >>((bufferSize - sourceLength)*8));
        }
        return retValue;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadLittleEndianInt32Unsafe(ReadOnlySpan<byte> source)
    {
        const int bufferSize = sizeof(int);
        var sourceLength=source.Length;
        var retValue=Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(source));
        if (BitConverter.IsLittleEndian)
        {
            var maskSize = (bufferSize - sourceLength) * 8;
            var mask = (1 << maskSize )- 1;
            retValue&= mask;
            return retValue;
            
            
            return retValue >>((bufferSize - sourceLength)*8);
        }
        return retValue;
    }



}