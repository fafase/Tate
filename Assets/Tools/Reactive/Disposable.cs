using System;

namespace Rx
{
    public class Disposable : IDisposable
    {
        public static readonly IDisposable Empty = new Disposable();
        public void Dispose() { }
    }
}
