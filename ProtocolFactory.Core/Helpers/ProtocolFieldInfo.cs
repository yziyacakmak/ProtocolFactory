using System.Reflection;
using ProtocolFactory.Core.Attributes;

namespace ProtocolFactory.Core.Helpers;

public class ProtocolFieldInfo
{
    public PropertyInfo Property { get; set; } = default!;
    public NumericProtocolFieldAttribute Attribute { get; set; } = default!;
    
}