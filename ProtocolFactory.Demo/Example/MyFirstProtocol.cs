using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtocolFactory.Core.Attributes;
using ProtocolFactory.Core.Contracts;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Demo.Example
{
    [Protocol]
    public partial class MyFirstProtocol
    {
      
        [ProtocolField(0,16,Endianness.Big)]
        public int First { get; set;}

    }
}
