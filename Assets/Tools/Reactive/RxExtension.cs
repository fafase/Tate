using System;
using UnityEngine.UI;

namespace Rx 
{
    public static class ButtonRxExtensions 
    {
        public static ISubject<Unit> OnClickAsObservable(this Button button)
        { 
            var subject = new Subject<Unit>();
            button.onClick.AddListener(() => subject.OnNext(Unit.Default));
            button.gameObject.AddComponent<DestroyNotifier>().OnDestroyed += () =>
            {
                subject.OnCompleted();
            };
            return subject;
        }
    }
}
