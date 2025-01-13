using UnityEngine;

namespace Racist
{
    [RequireComponent(typeof(AudioSource))]
    public class UIButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioClip m_Click;
        [SerializeField] private AudioClip m_Hover;

        private AudioSource m_Source;

        private UIButton[] m_Buttons;

        private void Start()
        {
            m_Source = GetComponent<AudioSource>();

            m_Buttons = GetComponentsInChildren<UIButton>(true);

            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].PointerEnter += OnPointerEnter;
                m_Buttons[i].PointerClick += OnPointerClick;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].PointerEnter -= OnPointerEnter;
                m_Buttons[i].PointerClick -= OnPointerClick;
            }
        }

        private void OnPointerEnter(UIButton button)
        {
            m_Source.PlayOneShot(m_Hover);
        }


        private void OnPointerClick(UIButton button)
        {
            m_Source.PlayOneShot(m_Click);
        }
    }
}
