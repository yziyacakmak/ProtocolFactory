using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ProtocolFieldAttribute(int startBit, int length, Endianness endian) : Attribute
{
    public readonly int StartBit = startBit;
    public readonly int Length = length;
    public readonly Endianness Endian = endian;
}
