using System;

namespace Rx
{
    public class ReactiveProperty<T> : IReactiveProperty<T>, IDisposable
    {
        private T value;

        public event Action<T> ValueChanged;

        public T Value
        {
            get => value;
            set
            {
                if (!Equals(this.value, value))
                {
                    this.value = value;
                    ValueChanged?.Invoke(this.value);
                }
            }
        }

        public ReactiveProperty(T initialValue = default)
        {
            value = initialValue;
        }

        public IDisposable Subscribe(Action<T> listener)
        {
            ValueChanged += listener;
            return new Unsubscriber(() => ValueChanged -= listener);
        }

        public void Dispose()
        {
            ValueChanged = null;
        }

        private class Unsubscriber : IDisposable
        {
            private readonly Action unsubscribeAction;

            public Unsubscriber(Action unsubscribeAction)
            {
                this.unsubscribeAction = unsubscribeAction;
            }

            public void Dispose()
            {
                unsubscribeAction();
            }
        }
    }
}