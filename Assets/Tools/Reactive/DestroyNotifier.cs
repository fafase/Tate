using System;
using UnityEngine;

namespace Rx 
{
    public class DestroyNotifier : MonoBehaviour
    {
        public event Action OnDestroyed;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }
    }
}
