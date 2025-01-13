using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Racist
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] protected bool IsInteractable;

        private bool m_Focused = false;
        public bool Focused => m_Focused;

        public UnityEvent OnClick;

        public event UnityAction<UIButton> PointerEnter;
        public event UnityAction<UIButton> PointerExit;
        public event UnityAction<UIButton> PointerClick;



        public virtual void SetFocuse()
        {
            if (!IsInteractable) return;

            m_Focused = true;
        }

        public virtual void SetUnfocuse()
        {
            if (!IsInteractable) return;

            m_Focused = false;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInteractable) return;

            PointerEnter?.Invoke(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!IsInteractable) return;

            PointerExit?.Invoke(this);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable) return;

            PointerClick?.Invoke(this);

            OnClick?.Invoke();
        }

        public virtual void ChangeInteractability(bool inter) => IsInteractable = inter;

        
    }
}
