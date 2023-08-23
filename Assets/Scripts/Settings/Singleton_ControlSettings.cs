using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace ScorgedEarth
{
    public class Singleton_ControlSettings : MonoSingleton<Singleton_ControlSettings>
    {
        public event UnityAction OnLeftMouseButtonPressed;
        public event UnityAction OnRightMouseButtonPressed;
        public event UnityAction InvButtonPressed;
        public event UnityAction PauseMenuButtonPressed;

        private bool m_IBPressed = false;
        private bool m_PMBPressed = false;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) OnLeftMouseButtonPressed?.Invoke();
            if (Input.GetMouseButtonDown(1)) OnRightMouseButtonPressed?.Invoke();
            if (Input.GetAxis("Inventory Open") >= 0.1 && !m_IBPressed)
            {
                m_IBPressed = true;
                InvButtonPressed?.Invoke();
            }
            else if (Input.GetAxis("Inventory Open") <= 0.1 && m_IBPressed) m_IBPressed = false;

            if (Input.GetAxis("Cancel") != 0 && !m_PMBPressed)
            {
                m_PMBPressed = true;
                PauseMenuButtonPressed?.Invoke();
            }
            else if (Input.GetAxis("Cancel") == 0 && m_PMBPressed) m_PMBPressed = false;
        }
    }
}
