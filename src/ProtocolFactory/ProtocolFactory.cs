using ProtocolFactory.Attributes;
using ProtocolFactory.Enums;
using ProtocolFactory.Models;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolFactory
{
    public static class ProtocolFactory
    {
        public static byte[] Create(IProtocol messageProtocol)
        {
            var dataPgc64 = BigInteger.Zero;
            //Int64 dataPgc64 = 0;
            var totalSize = 0;

            Type type = messageProtocol.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var field = property.GetCustomAttribute<ProtocolField>();
                if (field != null)
                {
                    totalSize += (int)field.Length;
                    var lengthMask = (BigInteger.One << (int)field.Length) - 1;
                    var propertyValue = property.GetValue(messageProtocol);

                    BigInteger intValue = new(Convert.ToInt64(propertyValue));
                    dataPgc64 |= (intValue & lengthMask) << (int)field.StartIndex;

                }
            }

            var totalBytes = (int)Math.Ceiling(totalSize / 8.0) * 8;
            byte[] bytes = dataPgc64.ToByteArray();
            Array.Resize(ref bytes, totalBytes / 8);
            return bytes;
        }

        public static void Read(ref ReadOnlySpan<byte> data, IProtocol messageProtocol)
        {
            Type type = messageProtocol.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ProtocolField>();
                if (attribute != null)
                {
                    var fieldSize = (int)Math.Ceiling(attribute.Length / 8.0) * 8;
                    ReadOnlySpan<byte> passingData;
                    double value = 0;
                    var propertyType = property.PropertyType;
                    if (property.GetValue(messageProtocol) is IList<object> fields)
                    {

                        Read(ref data, fields, ref attribute);


                    }
                    else if (property.GetValue(messageProtocol) is IProtocol nestedMessage)
                    {

                        passingData = data.Slice((int)attribute.StartIndex / 8, fieldSize / 8);
                        Read(ref passingData, nestedMessage);
                    }
                    else if (propertyType.IsPrimitive || propertyType.IsEnum)
                    {
                        value = GetPrimativeValue(ref data, ref attribute);


                        if (property.CanWrite)
                        {
                            if (propertyType == typeof(byte))
                            {
                                property.SetValue(messageProtocol, (byte)value);
                            }
                            else if (propertyType == typeof(short))
                            {
                                property.SetValue(messageProtocol, (short)value);
                            }
                            else if (propertyType == typeof(ushort))
                            {
                                property.SetValue(messageProtocol, (ushort)value);
                            }
                            else if (propertyType == typeof(double))
                            {
                                property.SetValue(messageProtocol, value);
                            }

                        }


                    }
                }
            }
        }

        private static void Read(ref ReadOnlySpan<byte> data, IEnumerable<object> fieldList, ref ProtocolField attribute)
        {
            var itemLength = (int)attribute.ItemLength;
            var startIndex = (int)attribute.StartIndex;
            foreach (var field in fieldList)
            {

                var type = field.GetType();
                if (field is IEnumerable<object> fields)
                {
                    Read(ref data, fields, ref attribute);
                }

                else if (field is IProtocol msgProtocol)
                {
                    var refData = data.Slice(startIndex / 8, itemLength / 8);
                    Read(ref refData, msgProtocol);
                    startIndex += itemLength;
                }

            }

        }

        private static double GetPrimativeValue(ref ReadOnlySpan<byte> data, ref ProtocolField attribute)
        {
            var fieldSize = (int)Math.Ceiling(attribute.Length / 8.0) * 8;
            var processedBitCounter = 0;
            var processedByteCounter = 0;
            double value = 0;
            var startIndex = (int)attribute.StartIndex / 8;

            if ((attribute.StartIndex + attribute.Length) % 8 == 0)
            {
                switch (attribute.Length)
                {
                    case 8:
                        value = data[startIndex];
                        break;
                    case 16:
                        value = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(startIndex, fieldSize / 8));
                        break;
                    case 32:
                        if (attribute.Value == NumberType.Integer)
                        {
                            value = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(startIndex, fieldSize / 8));
                        }
                        else
                        {
                            value = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(startIndex, fieldSize / 8));
                        }
                        break;
                    case 64:
                        value = BinaryPrimitives.ReadInt64LittleEndian(data.Slice(startIndex, fieldSize / 8));
                        break;

                }

            }
            else
            {

                var bitIndex = (int)attribute.StartIndex;
                for (int i = 0; i < attribute.Length; i++)
                {
                    var byteIndex = bitIndex / 8;
                    var bitInByteIndex = bitIndex % 8;
                    var bitValue = GetBit(data[byteIndex], bitInByteIndex);
                    bitIndex++;
                    value = ((int)value) | (bitValue << processedBitCounter);
                    processedBitCounter++;

                    if ((processedBitCounter & 7) == 0)
                    {
                        processedByteCounter++;
                        bitIndex = (int)attribute.StartIndex + processedByteCounter * 8;
                    }
                }
            }

            return (value * attribute.Factor) + attribute.Offset;

        }


        public static int GetBit(byte b, int index)
        {
            int mask = (1 << index);
            return (b & mask) >> index;
        }
    }
}
