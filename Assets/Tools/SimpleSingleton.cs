using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;
        void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }
            s_instance = this as T;
        }
    }
}
