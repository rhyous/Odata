using System;

namespace Rhyous.Odata
{
    public interface IHandler<T>
    {
        Action<T> Action { get; }
    }
}
