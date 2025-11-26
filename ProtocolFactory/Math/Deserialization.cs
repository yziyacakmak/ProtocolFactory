using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ProtocolFactory.Core.Contracts;

namespace ProtocolFactory.Core.Math;


public static class Deserialization
{
#if NET9_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
public unsafe static void Deserialize<T, TProtoValue>(ref T instance, ReadOnlySpan<byte> source)
    where T : class,IProtocol<T>
    where TProtoValue : struct, IProtocolValue<TProtoValue>
        {
            

            
            //Console.WriteLine("Using ReadOnlySpan<byte> overload");
            
        }
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void Deserialize<T, TProtoValue>(ref T instance, byte[] source)
       where T : class, IProtocol<T>
        where TProtoValue : struct, IProtocolValue<TProtoValue>
    {
        for(int i=0;i<instance.Setters.Length;i++)
        {
            


            instance.Setters[i](instance,0);
        }

        Console.WriteLine("Using byte[] overload");

    }
#endif


}

