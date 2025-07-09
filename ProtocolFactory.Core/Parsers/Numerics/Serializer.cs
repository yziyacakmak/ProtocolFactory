using ProtocolFactory.Core.Helpers;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Core.Parsers.Numerics;

public class Serializer<T> where T:INumericProto
{
    private readonly List<NumericProtocolFieldInfo> _fields = NumericProtocolHelper.GetNumericFieldInfos<T>();
    private readonly Dictionary<uint, Func<object, object>>  _getters = NumericProtocolHelper.FieldGetters<T>();
    
    public byte[] Serialize(T instance)
    {
        
        return null;
    }
    
}