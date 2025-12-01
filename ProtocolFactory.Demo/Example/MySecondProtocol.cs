using ProtocolFactory.Core.Attributes;
using ProtocolFactory.Core.Math;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Demo.Example;

[Protocol]
public sealed partial class MySecondProtocol
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
    
    public void DeserializeInstance(ReadOnlySpan<byte> source)
        {
            var FirstResult=(int)source[0];
            FirstResult &= 0xFF;
            First=(int)FirstResult;
            var SecondResult=(int)source[1];
            SecondResult &= 0xFF;
            SecondResult >>= 5;
            Second=(int)SecondResult;
            var ThirdResult=(int)source[1];
            ThirdResult &= 0x1F;
            Third=(int)ThirdResult;
            var FourthResult=(int)source[2];
            FourthResult &= 0xFF;
            FourthResult >>= 1;
            Fourth=(int)FourthResult;
            var FifthResult=ProtocolPrimitives.ReadBigEndianInt32Unsafe(source.Slice(2, 2));
            FifthResult &= 0x1FE;
            FifthResult >>= 1;
            Fifth=(int)FifthResult;
            var SixthResult=(int)source[3];
            SixthResult &= 0x1;
            Sixth=(int)SixthResult;
            var SeventhResult=(int)source[4];
            SeventhResult &= 0xFF;
            SeventhResult >>= 4;
            Seventh=(int)SeventhResult;
            var EighthResult=(int)source[4];
            EighthResult &= 0xF;
            Eighth=(int)EighthResult;
            var NinthResult=(int)source[5];
            NinthResult &= 0xFF;
            Ninth=(int)NinthResult;
            var TenthResult=(int)source[6];
            TenthResult &= 0xFF;
            TenthResult >>= 6;
            Tenth=(int)TenthResult;
            var EleventhResult=(int)source[6];
            EleventhResult &= 0x3F;
            Eleventh=(int)EleventhResult;
            var TwelfthResult=(int)source[7];
            TwelfthResult &= 0xFF;
            TwelfthResult >>= 5;
            Twelfth=(int)TwelfthResult;
            var ThirteenthResult=(int)source[7];
            ThirteenthResult &= 0x1F;
            ThirteenthResult >>= 2;
            Thirteenth=(int)ThirteenthResult;
            var FourteenthResult=(int)source[7];
            FourteenthResult &= 0x3;
            Fourteenth=(int)FourteenthResult;
        }
}