using System;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ProtocolFactory.Demo;

[MemoryDiagnoser]
public class Benchmark
{


    [GlobalSetup]
    public void Setup()
    {


    }

    [Benchmark]
    public void Loop_Pext()
    {

    }


}
