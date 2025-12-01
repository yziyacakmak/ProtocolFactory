using ProtocolFactory.Core.Attributes;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Demo.Example;

[Protocol]
public partial class MyLittleEndianProtocol
{
    [ProtocolField(13,6,Endianness.Little)]
    public int First { get; set; }
    
    [ProtocolField(11,8,Endianness.Big)]
    public int Second { get; set; }
    
}