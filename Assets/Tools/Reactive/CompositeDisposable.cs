using System;
using System.Collections.Generic;
namespace Rx 
{
    public class CompositeDisposable : IDisposable
    {
        private List<IDisposable> m_disposables = new List<IDisposable>();
        private bool m_isDisposed = false;

        public void Add(IDisposable disposable)
        {
            if (!m_isDisposed)
            {
                m_disposables.Add(disposable);
            }
            else
            {
                disposable.Dispose();
            }
        }

        public void Dispose()
        {
            if (!m_isDisposed)
            {
                foreach (var disposable in m_disposables)
                {
                    disposable.Dispose();
                }
                m_disposables.Clear();
                m_isDisposed = true;
            }
        }
    }

}