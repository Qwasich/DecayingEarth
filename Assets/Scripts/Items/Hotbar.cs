using UnityEngine;
using UnityEngine.UI;

namespace DecayingEarth
{
    public class Hotbar : MonoBehaviour
    {
        [SerializeField] private InvEntryPoint m_InventoryPoint;
        [SerializeField] private Text m_CurrentItemText;

        private int m_ActiveCell = 0;
        /// <summary>
        /// ������� �������� ������ �������
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

        /// <summary>
        /// �������� ������ �������, ����� �� ���������� (� ������� ��������� � ���������� ������)
        /// </summary>
        private void UpdateHotbarCell()
        {
            m_InventoryPoint.ButtonArray[m_ActiveCell].UnHighlightButton();
            m_ActiveCell = m_RequestCell;
            m_InventoryPoint.ButtonArray[m_ActiveCell].HighlightButton();
            UpdateTextCurrentCell();


        }

        private void UpdateButtonByIndex(int i)
        {
            m_RequestCell = i;
            UpdateHotbarCell();
        }

        /// <summary>
        /// ��������� ����� ���������
        /// </summary>
        public void UpdateTextCurrentCell()
        {
            if (m_CurrentItemText != null)
            {
                if (m_InventoryPoint.Inventory.Items[m_ActiveCell].Item == null) m_CurrentItemText.text = "Inventory";
                if (m_InventoryPoint.Inventory.Items[m_ActiveCell].Item != null) m_CurrentItemText.text = m_InventoryPoint.Inventory.Items[m_ActiveCell].Item.Name;
            }
        }


    }
}
