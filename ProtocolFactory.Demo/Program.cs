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

var littleProtocol = new MyLittleEndianProtocol();
littleProtocol.DeserializeInline(protocolData);
var startBit = 8;
var length = 4;
var msb=BitExchange.LsbToMsbLittleEndian(startBit,length);
Console.WriteLine(msb);

var mask=BitExchange.MaskCalculationLittleEndian(startBit,length);
Console.WriteLine($"0x{mask:X}");
var res=LittleEndianDeserialize(protocolData, startBit, length);
Console.WriteLine(res);
// var bbbb= new MySecondProtocol();
//
//
// bbbb.DeserializeInline(protocolData);
// Console.WriteLine(bbbb.First);


BenchmarkRunner.Run<Benchmark>();

int LittleEndianDeserialize(ReadOnlySpan<byte> source, int startBit, int length)
{
  var startByte = startBit / 8;
  var startBitInByte=startBit % 8;
  var msbBit=BitExchange.LsbToMsbLittleEndian(startBit,length);
  var endByte = msbBit / 8;
  var sourceAsByteLength = endByte - startByte + 1;
  var littleSource=source.Slice(startByte,sourceAsByteLength);
  var value=ProtocolPrimitives.ReadLittleEndianInt32Unsafe(littleSource);
  value&=BitExchange.MaskCalculationLittleEndian(startBit,length);
  value >>= BitExchange.ShiftAmountLittleEndian(startBit);
  return value;
}