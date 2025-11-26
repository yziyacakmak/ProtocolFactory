using System;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using ProtocolFactory.Demo.Example;

namespace ProtocolFactory.Demo;

[MemoryDiagnoser]
public class Benchmark
{
    private byte[] protocolData;
    private MyFirstProtocol protocolInstance;
    // 1. Consumer örneğini tanımlayın
    private readonly Consumer consumer = new Consumer();

    [GlobalSetup]
    public void Setup()
    {
        protocolData = new byte[] { 0x01, 0x2C, 0xFF, 0xEE };
        protocolInstance = new MyFirstProtocol();
    }

    [Benchmark]
    public void YaHak()
    {
        // 1. Deserialization'ı çağır
        protocolInstance.Deserialize(protocolData);

        // 2. JIT'ın kodu optimize etmesini önlemek için, 
        // Deserialization'ın sonucunda değişen bir alanı kullanın veya döndürün.
        // MyFirstProtocol'ün First alanının değiştiğini varsayalım.
        // BenchmarkDotNet'in 'Consume' metodunu kullanmak en iyi yoldur.
        consumer.Consume(protocolInstance.First);
    }


}
