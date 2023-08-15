using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace ScorgedEarth
{
    [RequireComponent(typeof(Sprite))]
    public class PhysicalItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_ItemSprite;
        [SerializeField] private float m_PickupDelay;
        private float m_Time;
        private InvItem m_HandledItem;
        public InvItem HandledItem => m_HandledItem;

        private bool m_IsPickableUp = true;
        public bool IsPickableUp => m_IsPickableUp;

        private void Update()
        {
            if (m_IsPickableUp) return;

            m_Time -= Time.deltaTime;
            if (m_Time <= 0) m_IsPickableUp = true;
        }

        /// <summary>
        /// Инициализирует параметры предмета.
        /// </summary>
        /// <param name="item">Предмет, прикрепленный к оъекту</param>
        /// <param name="count">Количество предметов в стаке</param>
        /// <param name="pick">Может ли предмет быть подобран сразу же. Если False - должна пройти задержка, чем игрок сможет его подобрать.</param>
        public void InitiateItem(ItemBase item, int count = 1,bool pick = true)
        {
            if (item == null) { Debug.LogError("ItemBase isn't assigned to the spawning parent!");return; }
            m_HandledItem = new InvItem(item, count);

            if (m_HandledItem.Item.Icon == null) { Debug.LogError("Sprite isn't set up on the ItemBase prefab: " + m_HandledItem.Item.name); return; }
            m_ItemSprite.sprite = m_HandledItem.Item.Icon;

            if (pick) return;
            m_IsPickableUp = false;
            m_Time = m_PickupDelay;
        }

        public void ReduceItemCount(int count)
        {
            if (m_HandledItem.StackCount < count) return;
            if (m_HandledItem.StackCount - count == 0)
            {
                Destroy(gameObject);
                return;
            }

            m_HandledItem = new InvItem(m_HandledItem.Item, m_HandledItem.StackCount - count);
        }

        public void SetItemCount(int count)
        {
            m_HandledItem = new InvItem(m_HandledItem.Item, count);
            if (m_HandledItem.StackCount - count == 0)
            {
                Destroy(gameObject);
                return;
            }
        }

    }
}
