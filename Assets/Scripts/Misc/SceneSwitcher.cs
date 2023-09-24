using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DecayingEarth
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private string m_SceneToLoad;

        public void LoadScene()
        {
            SceneManager.LoadScene(m_SceneToLoad);
        }

        public void CloseApplication()
        {
            Application.Quit();
        }
    }
}
