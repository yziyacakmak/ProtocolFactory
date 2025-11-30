using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Core.Contracts;

public interface IProtocolValue<TProto> where TProto : struct, IProtocolValue<TProto>
{
    int Length { get; }
    int FieldCount { get; }
    int[] StartBits { get; }
    int[] Lengths { get; }
    int[] Masks { get; }
    int[] Shifts { get; }
    int[] LengthAsByte { get; }
    Endianness[] Endians { get; }

}
