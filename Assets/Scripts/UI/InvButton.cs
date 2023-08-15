using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScorgedEarth
{
    public class InvButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private InvEntryPoint m_Ivp;
        [SerializeField] private Image m_ItemIcon;
        [SerializeField] private Text m_ItemCount;

        private int m_Id = -1;
        public int Id => m_Id;


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Сделать так, чтоб показывался Tooltip предмета
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Сделать так, чтобы скрывался Tooltip предмета
        }

        // Левая - 0, Правая - 1
        public void OnPointerClick(PointerEventData eventData) => m_Ivp.MakeActionDependingOnClickType((int)eventData.button, m_Id);

        public void Initiate(int id, InvEntryPoint inv)
        {
            m_Id = id;
            m_Ivp = inv;
        }

        public void UpdateButtonGraphics()
        {
            if (m_Ivp.Inventory == null)
            {
                Debug.LogError("Inventory wasn't set upon accessing it! Object: " + m_Ivp.gameObject);
                return;
            }

            if (Id == -1) return;

            if (m_ItemIcon == null)
            {
                Debug.LogError("Item Icon object isn't set on: " + gameObject);
            }

            if (m_Ivp.Inventory.Items[m_Id].Item != null && m_ItemIcon != null) m_ItemIcon.sprite = m_Ivp.Inventory.Items[m_Id].Item.Icon;
            if (m_Ivp.Inventory.Items[m_Id].Item == null && m_ItemIcon != null) m_ItemIcon.sprite = Singleton_PrefabLibrary.EmptySprite;
            if (m_Ivp.Inventory.Items[m_Id].StackCount > 1 && m_ItemCount != null) m_ItemCount.text = m_Ivp.Inventory.Items[m_Id].StackCount.ToString();
            if (m_Ivp.Inventory.Items[m_Id].StackCount <= 1 && m_ItemCount != null) m_ItemCount.text = "";

        }
    }
}
