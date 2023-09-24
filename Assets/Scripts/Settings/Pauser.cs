using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Racist
{
    public class Pauser : MonoBehaviour
    {
        public event UnityAction<bool> PauseStateChange;

        private bool m_IsPause;
        public bool IsPause => m_IsPause;

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        }

        private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode) => Unpause();

        public void ChangePauseState()
        {
            if (m_IsPause) Unpause();
            if (!m_IsPause) Pause();

        }

        public void Pause()
        {
            if (m_IsPause) return;

            Time.timeScale = 0.0f;
            m_IsPause = true;
            PauseStateChange?.Invoke(m_IsPause);
        }

        public void Unpause()
        {
            if (!m_IsPause) return;

            Time.timeScale = 1f;
            m_IsPause = false;
            PauseStateChange?.Invoke(m_IsPause);
        }

        
    }
}
