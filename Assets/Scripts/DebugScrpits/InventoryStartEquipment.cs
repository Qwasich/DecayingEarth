using UnityEngine;

namespace DecayingEarth
{
    public class InventoryStartEquipment : MonoBehaviour
    {
        [SerializeField] private InvEntryPoint m_PlayerInventory;
        [SerializeField] private InvItem[] m_StartingItems;


        private void Start()
        {
            for (int i = 0; i < m_StartingItems.Length; i++)
            {
                if (m_StartingItems[i].Item == null) continue;
                Vector2 pos = Camera.main.transform.position;
                GameObject item = Instantiate(Singleton_PrefabLibrary.Instance.DummyItemPrefab, pos, Quaternion.identity);
                PhysicalItem pi = item.GetComponent<PhysicalItem>();
                if (m_StartingItems[i].StackCount > m_StartingItems[i].Item.MaxStackCount) pi.InitiateItem(m_StartingItems[i].Item, m_StartingItems[i].Item.MaxStackCount, true);
                else pi.InitiateItem(m_StartingItems[i].Item, m_StartingItems[i].StackCount, true);

            }
        }

    }
}
