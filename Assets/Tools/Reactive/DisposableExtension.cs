using System;

namespace Rx
{
    public static class DisposableExtensions
    {
        public static IDisposable AddTo(this IDisposable disposable, CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Add(disposable);
            return disposable;
        }
    }
}
