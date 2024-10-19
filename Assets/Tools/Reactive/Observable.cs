using System;
namespace Rx 
{
    public class Observable<T> : IObservable<T>
    {
        private readonly IReactiveProperty<T> m_reactiveProperty;
        private readonly Func<T, bool> m_predicate;

        public Observable(IReactiveProperty<T> reactiveProperty, Func<T, bool> predicate)
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

            return m_reactiveProperty.Subscribe(value =>
            {
                if (m_predicate(value))
                {
                    observer.OnNext(value);
                }
            });
        }
    }
}