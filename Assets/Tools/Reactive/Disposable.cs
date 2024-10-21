using System;

namespace Rx
{
    public class Disposable : IDisposable
    {
        public static readonly IDisposable Empty = new Disposable();
        private Action m_disposeAction;
        private bool m_isDisposed;

        private Disposable() { }
        public Disposable(Action disposeAction)
        {
            this.m_disposeAction = disposeAction;
        }
        public static IDisposable Create(Action disposeAction)
        {
            return new Disposable(disposeAction);
        }

        public void Dispose() 
        {
            if (m_isDisposed) 
            {
                return;
            }
            m_isDisposed = true;
            m_disposeAction?.Invoke();
        }
    }
}
