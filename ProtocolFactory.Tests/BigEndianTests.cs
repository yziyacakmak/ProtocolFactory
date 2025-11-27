using ProtocolFactory.Core.Math;

namespace ProtocolFactory.Tests;

[TestClass]
public sealed class BigEndianTests
{
    [TestMethod]
    [DataRow(10, 13, 30)]   
    [DataRow(12, 10, 19)]   
    [DataRow(7, 17, 23)]   
    [DataRow(10, 3, 8)]    
    [DataRow(10, 11, 16)]  
    [DataRow(0, 8, 9)]     
    [DataRow(15, 16, 16)]   
    [DataRow(23, 8, 16)]    
    [DataRow(5, 20, 18)]    
    public void LsbCalculationBig(int msb, int length, int expectedLsb)
    {
        var actualLsb = BitExchange.MsbToLsbBigEndian(msb, length);
        Assert.AreEqual(expectedLsb, actualLsb, 
            $"msb={msb}, length={length}  lsb_expected={expectedLsb}, lsb_actual={actualLsb} ");
    }
    [TestMethod]
    [DataRow(10, 13, 0x07FFC0)]  
    [DataRow(12, 10, 0x1FF8)]    
    [DataRow(7, 17, 0xFFFF80)]   
    [DataRow(10, 3, 0x07)]     
    [DataRow(15, 16, 0xFFFF)]  
    [DataRow(0, 8, 0x1FE)]        
    public void MaskCalculation_ReturnsCorrectMask(int msbBit, int length, int expectedMask)
    {
        var actualMask = BitExchange.MaskCalculation(msbBit, length);
        Assert.AreEqual(expectedMask, actualMask, 
            $"MSB={msbBit}, Length={length}" +
            $"expected: 0x{expectedMask:X}, received: 0x{actualMask:X}");
    }
}