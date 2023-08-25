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
            if (Input.GetMouseButtonDown(0))
            {
                if(Singleton_SessionData.Instance != null) Singleton_SessionData.Instance.UpdateLastClick(IsClickOnCanvas());
                OnLeftMouseButtonPressed?.Invoke();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (Singleton_SessionData.Instance != null) Singleton_SessionData.Instance.UpdateLastClick(IsClickOnCanvas());
                OnRightMouseButtonPressed?.Invoke();
            }
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

        private bool IsClickOnCanvas()
        {
            Ray2D ray = new Ray2D(Input.mousePosition, transform.forward);
            RaycastHit2D[] hit;
            hit = Physics2D.RaycastAll(ray.origin, transform.forward);

            foreach(RaycastHit2D rh in hit)
            {
                CanvasRenderer cr;
                cr = rh.transform.gameObject.GetComponent<CanvasRenderer>();
                if (cr != null) return true;
            }

            return false;
        }
    }
}
