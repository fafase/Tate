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
        // A method to subscribe to IObservable<T> with only an Action for onNext.
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            // Create a simple observer that only implements onNext
            return source.Subscribe(
                new SimpleObserver<T>(onNext));
        }

        // Internal class implementing IObserver<T> to call Action<T> for onNext
        private class SimpleObserver<T> : IObserver<T>
        {
            private readonly Action<T> _onNext;

            public SimpleObserver(Action<T> onNext)
            {
                _onNext = onNext;
            }

            public void OnNext(T value)
            {
                _onNext(value); // Call the Action with the emitted value
            }

            public void OnError(Exception error)
            {
                
            }

            public void OnCompleted()
            {
               
            }
        }
    }
}