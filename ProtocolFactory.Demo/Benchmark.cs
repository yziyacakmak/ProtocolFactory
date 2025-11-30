using System;
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using ProtocolFactory.Core.Math;
using ProtocolFactory.Demo.Example;

namespace ProtocolFactory.Demo;

[MemoryDiagnoser]
public class Benchmark
{
    private byte[] protocolData;
    private byte[] protocolData2;
    private MyFirstProtocol protocolInstance;
    private MySecondProtocol protocolInstance2;
    private readonly Consumer consumer = new Consumer();
    private int mask = 0x7FFC0;
    private int shift = 6;

    [GlobalSetup]
    public void Setup()
    {
        protocolData = [0x01, 0x2C, 0xFF, 0xEE,0x01, 0x2C, 0xFF, 0xEE];
        protocolInstance2 = new MySecondProtocol();
    }
    
    [Benchmark]
    public void Generic()
    {
        protocolInstance2.Deserialize(protocolData);
        consumer.Consume(protocolInstance2);    
    }
    
    [Benchmark]
    public void Inline()
    {
        protocolInstance2.DeserializeInline(protocolData);
        consumer.Consume(protocolInstance2);    
    }

}
