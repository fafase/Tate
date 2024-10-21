using System;
using UnityEngine;

namespace Tate.Loading
{
    public class LoadingAnimation : MonoBehaviour
    {
        public event Action OnMiddleAnimationReach;
        public void TriggerWaitAnimation()
        {
            OnMiddleAnimationReach?.Invoke();
        }
    }
}
