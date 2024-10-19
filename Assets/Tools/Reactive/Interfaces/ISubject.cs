
using System;
namespace Rx 
{
    public interface ISubject<T> : IObserver<T>, IObservable<T>
    {
        IDisposable Subscribe(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null);
    } 
}