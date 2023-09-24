using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Racist
{
    public class UISelectableContainer : MonoBehaviour
    {
        [SerializeField] private Transform m_ButtonContainer;

        public bool Interactable = true;
        public void SetInteractable(bool interactable) => Interactable = interactable;

        private UISelectableButton[] m_Buttons;

        private int m_SelectedButtonIndex = 0;

        private void Start()
        {
            m_Buttons = m_ButtonContainer.GetComponentsInChildren<UISelectableButton>();

            if (m_Buttons == null) Debug.LogError("Button list is empty!");

            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].PointerEnter += OnPointerEnter;
            }

            if (!Interactable) return;

            m_Buttons[m_SelectedButtonIndex].SetFocuse();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].PointerEnter -= OnPointerEnter;
            }
        }

        private void OnPointerEnter(UIButton button)
        {
            SelectButton(button);
        }

        private void SelectButton(UIButton button)
        {
            if (!Interactable) return;

            m_Buttons[m_SelectedButtonIndex].SetUnfocuse();

            for (int i = 0; i < m_Buttons.Length; i++)
            {
                if (button == m_Buttons[i])
                {
                    m_SelectedButtonIndex = i;
                    button.SetFocuse();
                    break;
                }
            }
        }

        public void SelectNext()
        {

        }

        public void SelectPrevious()
        {

        }
    }
}
