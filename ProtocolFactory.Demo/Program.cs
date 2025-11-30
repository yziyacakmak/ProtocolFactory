// See https://aka.ms/new-console-template for more information

using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Running;
using ProtocolFactory.Core.Contracts;
using ProtocolFactory.Core.Math;
using ProtocolFactory.Demo;
using ProtocolFactory.Demo.Example;


var b = new Benchmark();

byte[] protocolData =[0x01, 0x2C, 0xFF, 0xEE,0x01, 0x2C, 0xFF, 0xEE];

var bbbb= new MySecondProtocol();


bbbb.DeserializeInline(protocolData);
Console.WriteLine(bbbb.First);


BenchmarkRunner.Run<Benchmark>();
