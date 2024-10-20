using System;

namespace Rx 
{
    public class ReactivePropertyObservable<T> : IObservable<T>
    {
        private readonly IReactiveProperty<T> m_reactiveProperty;
        private readonly Func<T, bool> m_predicate;

        public ReactivePropertyObservable(IReactiveProperty<T> reactiveProperty, Func<T, bool> predicate)
        {
            m_reactiveProperty = reactiveProperty;
            m_predicate = predicate;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (m_predicate(m_reactiveProperty.Value))
            {
                observer.OnNext(m_reactiveProperty.Value);
            }

            Action<T> onValueChanged = value =>
            {
                if (m_predicate(value))
                {
                    observer.OnNext(value);
                }
            };

            m_reactiveProperty.OnValueChanged += onValueChanged;

            return new Unsubscriber<T>(m_reactiveProperty, onValueChanged);
        }
        public IDisposable Subscribe(Action onNext)
        {
            Action<T> action = _ =>
            {
                onNext(); 
            };

            return Subscribe(new AnonymousObserver<T>(action));
        }

        private class AnonymousObserver<U> : IObserver<U>
        {
            private readonly Action<U> _onNext;

            public AnonymousObserver(Action<U> onNext)
            {
                _onNext = onNext;
            }

            public void OnNext(U value) => _onNext(value);
            public void OnError(Exception error) { /* Handle error if needed */ }
            public void OnCompleted() { /* Handle completion if needed */ }
        }
        public class Unsubscriber<U> : IDisposable
        {
            private readonly IReactiveProperty<U> _reactiveProperty;
            private readonly Action<U> _onValueChanged;

            public Unsubscriber(IReactiveProperty<U> reactiveProperty, Action<U> onValueChanged)
            {
                _reactiveProperty = reactiveProperty;
                _onValueChanged = onValueChanged;
            }

            public void Dispose()
            {
                _reactiveProperty.OnValueChanged -= _onValueChanged;
            }
        }
    }

    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            return source.Subscribe(
                new SimpleObserver<T>(onNext));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onComplete = null, Action<Exception> onError = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            return source.Subscribe(
                new SimpleObserver<T>(onNext, onComplete, onError));
        }
        private class SimpleObserver<T> : IObserver<T>
        {
            private readonly Action<T> m_onNext;
            private readonly Action m_onCompleted;
            private readonly Action<Exception> m_onError;

            public SimpleObserver(Action<T> onNext, Action onCompleted = null, Action<Exception> onError = null)
            {
                m_onNext = onNext;
                m_onCompleted = onCompleted;
                m_onError = onError;
            }

            public void OnNext(T value)
            {
                m_onNext(value); 
            }

            public void OnError(Exception error)
            {
                m_onError(error);
            }

            public void OnCompleted()
            {
               m_onCompleted();
            }
        }
    }
}