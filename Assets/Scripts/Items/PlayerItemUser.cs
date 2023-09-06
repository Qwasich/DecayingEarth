using UnityEngine;

namespace DecayingEarth
{
    public class PlayerItemUser : MonoBehaviour
    {
        [SerializeField] private Hotbar m_Hotbar;
        [SerializeField] private InvEntryPoint m_PlayerInventory;

        private float m_LastItemTimer = 0;
        /// <summary>
        /// Таймер от последнего использованного предмета. Пока не 0 - разрешение предмета снова не разрешено.
        /// </summary>
        public float LastItemTimer => m_LastItemTimer;

        private ItemBase m_LastUsedItem = null;

        private void Start()
        {
            Singleton_ControlSettings.Instance.OnLeftMouseButtonPressed += TryToUseItemLeftMouseButton;
            Singleton_ControlSettings.Instance.OnRightMouseButtonPressed += TryToUseItemRightMouseButton;
        }

        private void OnDestroy()
        {
            Singleton_ControlSettings.Instance.OnLeftMouseButtonPressed -= TryToUseItemLeftMouseButton;
            Singleton_ControlSettings.Instance.OnRightMouseButtonPressed -= TryToUseItemRightMouseButton;
        }

        private void Update()
        {
            if (m_LastItemTimer <= 0) return;

            m_LastItemTimer -= Time.deltaTime;

        }

        private void TryToUseItemLeftMouseButton()
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas)return;
            if (!AreUsedItemsTheSame()) m_LastItemTimer = 0;
            if (m_LastItemTimer > 0) return;
            if (Singleton_SessionData.Instance.IsInventoryHidden == false && Singleton_MouseItemHolder.Instance.HandItem.Item != null)
            {
                var item = Singleton_MouseItemHolder.Instance.HandItem.Item;
                int d = (item as UseItem).UseItem(0);
                m_LastItemTimer = item.UseTimer;
                m_LastUsedItem = item;
                Singleton_MouseItemHolder.Instance.DecreaseHandItemByNumber(d);
                Singleton_MouseItemHolder.Instance.UpdateHandVisual();
            }
            else
            {
                int hb = m_Hotbar.ActiveCell;
                if (m_PlayerInventory.Inventory.Items[hb].Item != null)
                {
                    var item = m_PlayerInventory.Inventory.Items[hb].Item;
                    int d = (item as UseItem).UseItem(0);
                    m_LastItemTimer = item.UseTimer;
                    m_LastUsedItem = item;
                    m_PlayerInventory.Inventory.DecreaseItemCount(hb, d);
                    m_PlayerInventory.UpdateButton(hb);
                    m_Hotbar.UpdateTextCurrentCell();
                }
            }
        }

        private void TryToUseItemRightMouseButton()
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return;
            if (Singleton_SessionData.Instance.IsInventoryHidden == false && Singleton_MouseItemHolder.Instance.HandItem.Item != null)
            {
                Singleton_MouseItemHolder.Instance.DropItem();
            }
            else
            {
                if (!AreUsedItemsTheSame()) m_LastItemTimer = 0;
                if (m_LastItemTimer > 0) return;
                int hb = m_Hotbar.ActiveCell;
                if (m_PlayerInventory.Inventory.Items[hb].Item != null)
                {
                    var item = m_PlayerInventory.Inventory.Items[hb].Item;
                    int d = (item as UseItem).UseItem(1);
                    m_LastItemTimer = item.UseTimer;
                    m_LastUsedItem = item;
                    m_PlayerInventory.Inventory.DecreaseItemCount(hb, d);
                    m_PlayerInventory.UpdateButton(hb);
                    m_Hotbar.UpdateTextCurrentCell();
                }

            }
        }

        private bool AreUsedItemsTheSame()
        {
            if (Singleton_SessionData.Instance.IsInventoryHidden == false && Singleton_MouseItemHolder.Instance.HandItem.Item != m_LastUsedItem) return false;
            if (m_PlayerInventory.Inventory.Items[m_Hotbar.ActiveCell].Item != m_LastUsedItem) return false;
            return true;
        }

    }
}
