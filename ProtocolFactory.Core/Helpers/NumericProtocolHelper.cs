using System.Linq.Expressions;
using System.Reflection;
using ProtocolFactory.Core.Attributes;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Core.Helpers;

public static class NumericProtocolHelper
{
    // public static List<ProtocolFieldInfo> GetFields<T>() where T : INumericProto
    // {
    //     return typeof(T)
    //         .GetProperties()
    //         .Select(p => new { Property = p, Attr = p.GetCustomAttribute<NumericProtocolFieldAttribute>() })
    //         .Where(x => x.Attr != null)
    //         .Select(x => new ProtocolFieldInfo
    //         {
    //             Property = x.Property,
    //             Attribute = x.Attr!
    //         })
    //         .ToList();
    // }
    
    public static List<NumericProtocolFieldInfo> GetNumericFieldInfos<T>()where T : INumericProto
    {
        
        var fields=typeof(T).GetProperties()
            .Select(p => new { Property = p, Attr = p.GetCustomAttribute<NumericProtocolFieldAttribute>() })
            .Where(x => x.Attr != null)
            .ToList();
        
        List<NumericProtocolFieldInfo> fieldInfos = new();

        foreach (var field in fields)
        {
            var length = (int)field.Attr!.Length;
            
            var isBigEndian = field.Attr.Endianness == Endianness.Big;
            
            var msbBit = isBigEndian ? (int)field.Attr.StartBit : Calculations.GetLittleEndianMsbIndex((int)field.Attr.StartBit,length);
            var lsbBit=isBigEndian ? Calculations.GetBigEndianLsbIndex(msbBit,length) : (int)field.Attr.StartBit;
            var msbByteIndex = msbBit / 8;
            var lsbByteIndex = lsbBit / 8;
            var msbBitInByte= msbBit % 8;
            var lsbBitInByte = lsbBit % 8;
            var msbByteMask = isBigEndian ? Calculations.GetMsbByteMask(msbBitInByte) : Calculations.GetLsbByteMask(lsbBitInByte);
            var lsbByteMask = isBigEndian ? Calculations.GetLsbByteMask(lsbBitInByte) : Calculations.GetMsbByteMask(msbBitInByte);
            var byteCount= Math.Abs(lsbByteIndex-msbByteIndex)+1;
            var fieldInfo = new NumericProtocolFieldInfo()
            {
                Length = length,
                IsBigEndian = isBigEndian,
                MsbBit = msbBit,
                LsbBit = lsbBit,
                MsbByteIndex = msbByteIndex,
                LsbByteIndex = lsbByteIndex,
                MsbByteMask = msbByteMask,
                LsbByteMask = lsbByteMask,
                ByteCount = byteCount,
                Offset = field.Attr.Offset,
                Factor = field.Attr.Factor,    
            };
            
            fieldInfos.Add(fieldInfo);
        }

        return fieldInfos;
    }
    
    
    public static Dictionary<uint, Func<object, object>> FieldGetters<T>() where T : INumericProto
    {
        var type= typeof(T);
        var result = new Dictionary<uint, Func<object, object>>();
        var parameter = Expression.Parameter(typeof(object), "obj");
        var castedTarget = Expression.Convert(parameter, type);

        var members = type
            .GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(m =>
                (m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field) &&
                m.GetCustomAttribute<NumericProtocolFieldAttribute>() is not null
            );

        foreach (var member in members)
        {
            var attr = member.GetCustomAttribute<NumericProtocolFieldAttribute>()!;
            Expression accessExpr = member switch
            {
                PropertyInfo pi => Expression.Property(castedTarget, pi),
                FieldInfo fi => Expression.Field(castedTarget, fi),
                _ => throw new NotSupportedException()
            };

            var convert = Expression.Convert(accessExpr, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(convert, parameter).Compile();

            result[attr.StartBit] = lambda;
        }

        return result;
    }
}