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
      
        [ProtocolField(15,8,Endianness.Big)]
        public int First { get; set;}

        [ProtocolField(16, 8, Endianness.Big)]
        public int Second { get; set; }


        // [ProtocolField(24, 8, Endianness.Little)]
        // public int Third { get; set; }

    }
}
