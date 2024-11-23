using System;
using BenchmarkDotNet.Attributes;
using ProtocolFactory.Benchmarks.Protocols;

namespace ProtocolFactory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class AllocationBenchmark1
{

   private byte[] data = new byte[] { 0x1, 0x2, 0x3, 0x0, 0x51,0x00,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
   private readonly Protocol1 proto = new Protocol1();

    [Benchmark]
    public void First()
    {
        ReadOnlySpan<byte> dataAsSpan = data;
        ProtocolFactory.Read(ref dataAsSpan, proto);
    }  
}
