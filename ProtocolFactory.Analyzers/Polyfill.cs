using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
    // Bu tip, C# 9.0 derleyicisinin 'init' accessor'ları için gereklidir.
    // Eski hedef çerçevelerde bulunmadığı için elle eklenir (Polyfill).
    internal static class IsExternalInit
    {
    }
}
