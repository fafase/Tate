using System;
using System.Collections.Generic;
namespace Rx 
{
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