using ProtocolFactory.Core.Attributes;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Demo.Example;

[Protocol]
public partial class MySecondProtocol
{
    [ProtocolField(7,8,Endianness.Big)]
    public int First { get; set;}

    [ProtocolField(15, 3, Endianness.Big)]
    public int Second { get; set; }
    
    [ProtocolField(12, 5, Endianness.Big)]
    public int Third { get; set; }
    
    [ProtocolField(23, 7, Endianness.Big)]
    public int Fourth { get; set; }
    
    [ProtocolField(16, 8, Endianness.Big)]
    public int Fifth { get; set; }
    
    [ProtocolField(24, 1, Endianness.Big)]
    public int Sixth { get; set; }
    
    [ProtocolField(39, 4, Endianness.Big)]
    public int Seventh { get; set; }
    
    
    [ProtocolField(35, 4, Endianness.Big)]
    public int Eighth { get; set; }
    
    [ProtocolField(47, 8, Endianness.Big)]
    public int Ninth { get; set; }
    
    [ProtocolField(55, 2, Endianness.Big)]
    public int Tenth { get; set; }
    
    [ProtocolField(53, 6, Endianness.Big)]
    public int Eleventh { get; set; }
    
    [ProtocolField(63, 3, Endianness.Big)]
    public int Twelfth { get; set; }
    
    [ProtocolField(60, 3, Endianness.Big)]
    public int Thirteenth { get; set; }
    [ProtocolField(57, 2, Endianness.Big)]
    public int Fourteenth {get; set; }
}