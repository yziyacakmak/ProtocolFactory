using System;
using ProtocolFactory.Attributes;
using ProtocolFactory.Models;

namespace ProtocolFactory.Benchmarks.Protocols;

public class Protocol1:IProtocol
{
   [ProtocolField("field1",0,8)]
   public byte Field1 { get; set; }
   
   [ProtocolField("field2",8,8)]
   public byte Field2 { get; set; }
   
   [ProtocolField("field3",16,8)]
   public byte Field3 { get; set; }
   
   [ProtocolField("field4",24,16)]
   public ushort Field4 { get; set; }
    
}
