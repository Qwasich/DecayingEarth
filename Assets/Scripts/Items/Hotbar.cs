using UnityEngine;
using UnityEngine.UI;

namespace ScorgedEarth
{
    public class Hotbar : MonoBehaviour
    {
        [SerializeField] private InvEntryPoint m_InventoryPoint;
        [SerializeField] private Text m_CurrentItemText;
        [SerializeField] private int m_HotbarSize = 9;

        private int m_ActiveCell = 0;
        /// <summary>
        /// Текущая активная ячейка хотбара
        /// </summary>
        public int ActiveCell => m_ActiveCell;

        private int m_RequestCell = 0;

        private void Start()
        {
            m_InventoryPoint.ReadyToUpdate += UpdateHotbarCell;
            Singleton_ControlSettings.Instance.HotbarButtonPressedByIndex += UpdateButtonByIndex;
        }

        private void OnDestroy()
        {
            m_InventoryPoint.ReadyToUpdate -= UpdateHotbarCell;
            Singleton_ControlSettings.Instance.HotbarButtonPressedByIndex -= UpdateButtonByIndex;
        }

        private void UpdateHotbarCell()
        {
            m_InventoryPoint.ButtonArray[m_ActiveCell].UnHighlightButton();
            m_ActiveCell = m_RequestCell;
            m_InventoryPoint.ButtonArray[m_ActiveCell].HighlightButton();
            if(m_CurrentItemText != null)
            {
                if (m_InventoryPoint.Inventory.Items[m_ActiveCell].Item == null) m_CurrentItemText.text = "Inventory";
                if (m_InventoryPoint.Inventory.Items[m_ActiveCell].Item != null) m_CurrentItemText.text = m_InventoryPoint.Inventory.Items[m_ActiveCell].Item.Name;
            }
        }

        private void UpdateButtonByIndex(int i)
        {
            m_RequestCell = i;
            UpdateHotbarCell();
        }


    }
}
