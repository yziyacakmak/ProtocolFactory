
namespace ProtocolFactory.Core.Contracts;

public interface IProtocol<T> where T : class
{
    unsafe delegate*<T, ulong, void>[] Setters { get; }
}

