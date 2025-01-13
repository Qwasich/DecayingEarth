using UnityEngine;
using UnityEngine.EventSystems;

namespace DecayingEarth
{
    public class ButtonManualCraft : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject m_ChestInterface;

        public void OnPointerClick(PointerEventData eventData)
        {
            Singleton_CraftingEntryPoint.Instance.InitiateCrafting();
            if (m_ChestInterface != null) m_ChestInterface.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
}
