using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Racist
{
    public class UISelectableButton : UIButton
    {
        [SerializeField] private Image m_SelectableImage;

        public UnityEvent OnSelect;
        public UnityEvent OnDeselect;

        protected virtual void Start()
        {
            if (m_SelectableImage != null) m_SelectableImage.enabled = false;
        }

        private void OnDisable()
        {
            if (m_SelectableImage != null) m_SelectableImage.enabled = false;
        }

        public override void SetFocuse()
        {
            base.SetFocuse();

            if (m_SelectableImage!=null) m_SelectableImage.enabled = true;
            OnSelect?.Invoke();
        }

        public override void SetUnfocuse()
        {
            base.SetUnfocuse();

            if (m_SelectableImage != null) m_SelectableImage.enabled = false;
            OnDeselect?.Invoke();
        }
    }
}
