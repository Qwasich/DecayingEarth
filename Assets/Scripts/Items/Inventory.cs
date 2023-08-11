using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScorgedEarth
{
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
        public void Load(Vector2Int tag, List<InvItem> itemList, int invSize);
    }

    public class Inventory : MonoBehaviour , ISaveableInventory
    {   
        private List<InvItem> m_Items;
        /// <summary>
        /// ���� ���� ��������� � ���������.
        /// </summary>
        public List<InvItem> Items => m_Items;

        private Vector2Int m_TagCoordinate;

        [SerializeField] private int m_InventorySize = 36;
        /// <summary>
        /// ������ ���������.
        /// </summary>
        public int InventorySize => m_InventorySize;

        [SerializeField] private bool m_IsSaveable = true;
        /// <summary>
        /// ���� true, ��������� ����� ��������.
        /// </summary>
        public bool IsSaveable => m_IsSaveable;

        private void Awake()
        {
            m_TagCoordinate = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
            m_Items = new List<InvItem>(m_InventorySize);
        }

        /// <summary>
        /// ��������� ����������� ��������� ���������� �������.
        /// </summary>
        /// <param name="state">���� True - ����� ����������, False - ���</param>
        public void SetSaveableState(bool state) => m_IsSaveable = state;

        /// <summary>
        /// ��������� ������ ���������. ����������� ��������� IsSaveable ������ ��� ��������
        /// </summary>
        /// <param name="tag">������-��� ���������</param>
        /// <param name="itemList">���� ��������� ���������</param>
        /// <param name="invSize">������ ���������</param>
        public void Save(out Vector2Int tag, out List<InvItem> itemList, out int invSize)
        {
            tag = m_TagCoordinate;
            itemList = m_Items;
            invSize = m_InventorySize;
        }

        /// <summary>
        /// ��������� ������ ���������. ����������� ��������� IsSaveable ������ ��� ��������
        /// </summary>
        /// <param name="tag">������-��� ���������</param>
        /// <param name="itemList">���� ��������� ���������</param>
        /// <param name="invSize">������ ���������</param>
        public void Load(Vector2Int tag, List<InvItem> itemList, int invSize)
        {
            m_TagCoordinate = tag;
            m_Items = itemList;
            m_InventorySize = invSize;
        }


        /// <summary>
        /// ����������� ������ ����� �������� � ��������� �� ������������ ������� �� ������������ ����������.
        /// </summary>
        /// <param name="invNumber">����� ����� ��������� ��� ���������</param>
        /// <param name="addAmount">����� ���������, ����������� � �����</param>
        /// <param name="usedAmount">���� ���������� ����������� ��������� ������ ��� ����������� �����, ���������� ���������� ������������� ���������</param>
        /// <returns>���������� True ���� ��������� �������; False - ���� ���.</returns>
        public bool IncreaseItemCount(int invNumber, int addAmount, out int usedAmount)
        {
            if (m_Items[invNumber].Item == null || addAmount <= 0)
            {
                usedAmount = 0;
                return false;
            }

            int stack = m_Items[invNumber].StackCount + addAmount;
            if (stack > m_Items[invNumber].Item.MaxStackCount)
            {
                usedAmount = stack - m_Items[invNumber].Item.MaxStackCount;
            }
            else
            {
                usedAmount = 0;
            }

            m_Items[invNumber] = new InvItem(m_Items[invNumber].Item, stack - usedAmount);

            return true;
        }

        /// <summary>
        /// ����������� ������ ����� �������� � ��������� �� ������������ ������� �� ������������ ����������.
        /// </summary>
        /// <param name="invNumber">����� ����� ��������� ��� ���������</param>
        /// <param name="substractAmount">����� ���������, ���������� �� �����</param>
        /// <returns>���������� True ���� ��������� �������; False - ���� ���.</returns>
        public bool DecreaseItemCount(int invNumber, int substractAmount)
        {
            if (m_Items[invNumber].Item == null || substractAmount >= 0 || m_Items[invNumber].StackCount < substractAmount) return false;

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
        /// ��������� ����� ������� � ��������� ������� ���������
        /// </summary>
        /// <param name="invNumber">����� ����� ��������� ��� ���������</param>
        /// <param name="item">����������� �������</param>
        public void AddNewItem(int invNumber, InvItem item)
        {
            m_Items[invNumber] = new InvItem(item.Item, item.StackCount);
        }

        /// <summary>
        /// ������� ��������� ������.
        /// </summary>
        /// <param name="invNumber">����� ����� ��������� ��� ���������</param>
        public void RemoveItemCompletely(int invNumber) => m_Items[invNumber] = new InvItem(null,0);
    }
}
