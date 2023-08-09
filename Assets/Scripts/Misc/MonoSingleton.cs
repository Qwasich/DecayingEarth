using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [DisallowMultipleComponent]
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool m_DoNotDestroyOnLoad;

        public static T Instance { get; private set; }

        #region Unity events

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this as T;

            if (m_DoNotDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        #endregion
    }

}