namespace ProtocolFactory.Core.Helpers;

public record NumericProtocolFieldInfo
{
    //if protocol field order is Big Endian Start Bit is MSB otherwise Start Bit is LSB
    public int Length { get; init; }
    public int MsbBit { get; init; }
    public int LsbBit { get; init; }
    public int MsbByteIndex { get; init; }
    public int LsbByteIndex { get; init; }
    public bool IsBigEndian { get; init; }
    public int MsbByteMask { get; init; }
    public int LsbByteMask { get; init; }
    public int ByteCount { get; init; }
    public double Offset { get; init; }
    public double Factor { get; init; }
    public int ShiftAmount { get; init; }
}