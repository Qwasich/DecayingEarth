using UnityEngine;

namespace DecayingEarth
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

        public void ChangeInventory()
        {
            if (Singleton_PlayerInfo.Instance.Player.CurrentHealth <= 0)
            {
                m_InventoryUI?.SetActive(false);
                m_State = true; Singleton_SessionData.Instance.UpdateInventoryVisibility(m_State);
                return;
            }

            m_InventoryUI?.SetActive(m_State);
            m_State = !m_State;
            Singleton_SessionData.Instance.UpdateInventoryVisibility(m_State);

            if (m_State)
            {
                Singleton_MouseItemHolder.Instance.DropItem();
                Singleton_CraftingEntryPoint.Instance.DisableCraftingUI();
                Singleton_MouseItemHolder.Instance.HideTooltip();
                Singleton_GlobalChestController.Instance.CloseInventory();
            }
        }

    }
}
