using System;
using System.Collections.Generic;
namespace Rx 
{
    public static class Observable 
    {
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe) 
        {
            return new AnonymousObservable<T>(subscribe);
        }
        private class AnonymousObservable<T> : IObservable<T> 
        {
            private readonly Func<IObserver<T>, IDisposable> m_subscribe;
            public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
            {
                m_subscribe = subscribe;
            }
            public IDisposable Subscribe(IObserver<T> observer)
            {
                var disposable =  m_subscribe(observer);

                return Disposable.Create(() =>
                {
                    UnityEngine.Debug.Log("Disposing of the original disposable.");
                    disposable.Dispose();
                });
            }
        }
    }
    public class Observable<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }

        public IDisposable Subscribe(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            var observer = new Observer<T>(onNext, onError, onCompleted);
            return Subscribe(observer);
        }

        public void Notify(T value)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(value);
            }
        }

        public void Complete()
        {
            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }
            observers.Clear(); 
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }
    }
}