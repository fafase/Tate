using System;

namespace Rx 
{
    public interface IReactiveProperty<T>
    {
        T Value { get; set; }
        event Action<T> ValueChanged;
        IDisposable Subscribe(Action<T> listener);
    }
}