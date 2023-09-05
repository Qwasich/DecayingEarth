using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScourgedEarth
{
    [Serializable]
    public struct InvItem
    {
        public ItemBase Item;
        public int StackCount;

        public InvItem(ItemBase item = null, int stackCount = 0)
        {
            Item = item;
            StackCount = stackCount;
        }
    }

    public interface ISaveableInventory
    {
        public void Save(out Vector2Int tag, out List<InvItem> itemList, out int invSize);
        public void ManualSave(out string tag, out List<InvItem> itemList, out int invSize);
        public void Load(Vector2Int tag, List<InvItem> itemList, int invSize);
        public void ManualLoad(string tag, List<InvItem> itemList, int invSize);
    }

    public class Inventory : MonoBehaviour , ISaveableInventory
    {   
        private List<InvItem> m_Items;
        /// <summary>
        /// Лист всех предметов в инвентаре.
        /// </summary>
        public List<InvItem> Items => m_Items;

        private Vector2Int m_TagCoordinate;
        [SerializeField] public Vector2Int TagCoordinate => m_TagCoordinate;

        [SerializeField] private int m_InventorySize = 36;
        /// <summary>
        /// Размер инвентаря.
        /// </summary>
        public int InventorySize => m_InventorySize;

        [SerializeField] private bool m_IsSaveable = true;
        /// <summary>
        /// Если true, инвентарь будет сохранен.
        /// </summary>
        public bool IsSaveable => m_IsSaveable;

        /// <summary>
        /// Если true - сохраняет по строчке тэга вместо координат.
        /// </summary>
        [SerializeField] private bool m_IsManual = false;
        /// <summary>
        /// Используется только при сохранении по строчке тэга. Полезно для объектов, заданных заранее (инвентарь игрока, например)
        /// </summary>
        [SerializeField] private string m_StringTag = "";
        public string StringTag => m_StringTag;


        private void Awake()
        {
            m_TagCoordinate = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
            m_Items = new List<InvItem>(m_InventorySize);

            for (int i = 0; i < m_Items.Capacity; i++)
            {
                m_Items.Add(new InvItem(null, 0));
            }
        }

        /// <summary>
        /// Позволяет переключить состояние сохранения объекта.
        /// </summary>
        /// <param name="state">Если True - будет сохранятся, False - нет</param>
        public void SetSaveableState(bool state) => m_IsSaveable = state;

        /// <summary>
        /// Сохраняет данные инвентаря. ОБЯЗАТЕЛЬНО проверьте IsSaveable прежде чем вызывать
        /// </summary>
        /// <param name="tag">Вектор-тэг инвентаря</param>
        /// <param name="itemList">Лист предметов инвентаря</param>
        /// <param name="invSize">Размер инвентаря</param>
        public void Save(out Vector2Int tag, out List<InvItem> itemList, out int invSize)
        {
            if (m_IsManual) Debug.LogError("Warning! Attempt to save inventory by coordinate tag instead of manual on object: " + name);
            tag = m_TagCoordinate;
            itemList = m_Items;
            invSize = m_InventorySize;
        }

        public void ManualSave(out string tag, out List<InvItem> itemList, out int invSize)
        {
            if (!m_IsManual) Debug.LogError("Warning! Attempt to save inventory by manual tag instead of coordinate tag on object: " + name);
            if (m_StringTag == "") Debug.LogError("Warning! Attempt to save inventory by empty manual tag on object: " + name);
            tag = m_StringTag;
            itemList = m_Items;
            invSize = m_InventorySize;
        }


        /// <summary>
        /// загружает данные инвентаря. ОБЯЗАТЕЛЬНО проверьте IsSaveable прежде чем вызывать
        /// </summary>
        /// <param name="tag">Вектор-тэг инвентаря</param>
        /// <param name="itemList">Лист предметов инвентаря</param>
        /// <param name="invSize">Размер инвентаря</param>
        public void Load(Vector2Int tag, List<InvItem> itemList, int invSize)
        {
            if (m_IsManual) Debug.LogError("Warning! Attempt to load inventory by coordinate tag instead of manual on object: " + name);
            m_TagCoordinate = tag;
            m_Items = itemList;
            m_InventorySize = invSize;
        }

        public void ManualLoad(string tag, List<InvItem> itemList, int invSize)
        {
            if (!m_IsManual) Debug.LogError("Warning! Attempt to load inventory by manual tag instead of coordinate tag on object: " + name);
            if (m_StringTag == "") Debug.LogError("Warning! Attempt to save inventory by empty manual tag on object: " + name);
            m_Items = itemList;
            m_InventorySize = invSize;

        }

        /// <summary>
        /// Увеличивает размер стака предмета в инвентаре на определенной позиции на определенное количество.
        /// </summary>
        /// <param name="invNumber">Номер слота инвентаря для изменения</param>
        /// <param name="addAmount">Число предметов, добавляемое к стаку</param>
        /// <param name="notUsedAmount">Если количество добавляемых предметов больше чем вместимость стака, возвращает количество не использованных предметов</param>
        /// <returns>Возвращает True если изменение успешно; False - если нет.</returns>
        public bool IncreaseItemCount(int invNumber, int addAmount, out int notUsedAmount)
        {
            if (m_Items[invNumber].Item == null || addAmount <= 0)
            {
                notUsedAmount = 0;
                return false;
            }

            int stack = m_Items[invNumber].StackCount + addAmount;
            if (stack > m_Items[invNumber].Item.MaxStackCount)
            {
                notUsedAmount = stack - m_Items[invNumber].Item.MaxStackCount;
            }
            else
            {
                notUsedAmount = 0;
            }

            m_Items[invNumber] = new InvItem(m_Items[invNumber].Item, stack - notUsedAmount);

            return true;
        }

        /// <summary>
        /// Увеличивает размер стака предмета в инвентаре на определенной позиции на определенное количество.
        /// </summary>
        /// <param name="invNumber">Номер слота инвентаря для изменения</param>
        /// <param name="substractAmount">Число предметов, вычитаемое из стака</param>
        /// <returns>Возвращает True если изменение успешно; False - если нет.</returns>
        public bool DecreaseItemCount(int invNumber, int substractAmount)
        {
            if (m_Items[invNumber].Item == null || substractAmount <= 0 || m_Items[invNumber].StackCount < substractAmount) return false;

            int stack = m_Items[invNumber].StackCount - substractAmount;

            if(stack > 0)
            {
                m_Items[invNumber] = new InvItem(m_Items[invNumber].Item, stack);
            }
            else
            {
                RemoveItemCompletely(invNumber);
            }

            return true;
        }

        /// <summary>
        /// Добавляет новый предмет в выбранную позицию инвентаря
        /// </summary>
        /// <param name="invNumber">Номер слота инвентаря для изменения</param>
        /// <param name="item">Добавляемый предмет</param>
        public void AddNewItem(int invNumber, InvItem item)
        {
            m_Items[invNumber] = new InvItem(item.Item, item.StackCount);
        }

        /// <summary>
        /// Очищает выбранную ячейку.
        /// </summary>
        /// <param name="invNumber">Номер слота инвентаря для изменения</param>
        public void RemoveItemCompletely(int invNumber) => m_Items[invNumber] = new InvItem(null,0);

        
    }
}
