using System;

namespace Rx 
{
    public interface IReactiveProperty<T>
    {
        T Value { get; set; }
        event Action<T> OnValueChanged;
        IDisposable Subscribe(Action<T> listener);
    }
}