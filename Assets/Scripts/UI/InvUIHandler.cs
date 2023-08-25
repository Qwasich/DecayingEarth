using UnityEngine;

namespace ScorgedEarth
{
    public class InvUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject m_InventoryUI;
        private bool m_State;

        private void Start()
        {
            if (m_InventoryUI.activeSelf) m_InventoryUI?.SetActive(false);
            m_State = m_InventoryUI.activeSelf;
            Singleton_ControlSettings.Instance.InvButtonPressed += ChangeInventory;
            ChangeInventory();
        }

        private void OnDestroy()
        {
            Singleton_ControlSettings.Instance.InvButtonPressed -= ChangeInventory;
        }

        private void ChangeInventory()
        {
            m_InventoryUI?.SetActive(m_State);
            m_State = !m_State;
            Singleton_SessionData.Instance.UpdateInventoryVisibility(m_State);
        }
    }
}
