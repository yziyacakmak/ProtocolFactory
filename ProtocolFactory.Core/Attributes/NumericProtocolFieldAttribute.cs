using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class NumericProtocolFieldAttribute(uint startBit,uint length,double factor=1,int offset=0,Endianness endianness=Endianness.Little):Attribute
{
    public Endianness Endianness { get; } = endianness;
    public uint StartBit { get; } = startBit;
    public uint Length { get; } = length;
    public double Factor { get; } = factor;
    public int Offset { get; } = offset;
}