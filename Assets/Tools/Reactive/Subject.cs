using System;
using System.Collections.Generic;

namespace Rx
{
    public class Subject<T> : ISubject<T>
    {
        private readonly List<IObserver<T>> m_observers = new List<IObserver<T>>();
        private bool m_isCompleted = false;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (m_isCompleted)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            m_observers.Add(observer);
            return new Unsubscriber(m_observers, observer);
        }

        public IDisposable Subscribe(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            var observer = new Observer<T>(onNext, onError, onCompleted);
            return Subscribe(observer);
        }

        public void OnNext(T value)
        {
            if (!m_isCompleted)
            {
                foreach (var observer in m_observers)
                {
                    observer.OnNext(value);
                }
            }
        }

        public void OnError(Exception error)
        {
            if (!m_isCompleted)
            {
                foreach (var observer in m_observers)
                {
                    observer.OnError(error);
                }
                m_observers.Clear();
                m_isCompleted = true;
            }
        }

        public void OnCompleted()
        {
            if (!m_isCompleted)
            {
                foreach (var observer in m_observers)
                {
                    observer.OnCompleted();
                }
                m_observers.Clear();
                m_isCompleted = true;
            }
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

    public class Observer<T> : IObserver<T>
    {
        private readonly Action<T> m_onNext;
        private readonly Action<Exception> m_onError;
        private readonly Action m_onCompleted;

        public Observer(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            m_onNext = onNext;
            m_onError = onError;
            m_onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            m_onNext?.Invoke(value);
        }

        public void OnError(Exception error)
        {
            m_onError?.Invoke(error);
        }

        public void OnCompleted()
        {
            m_onCompleted?.Invoke();
        }
    }
}
