using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScorgedEarth
{
    [RequireComponent(typeof(Sprite))]
    public class PhysicalItem : MonoBehaviour
    {
        [SerializeField]private SpriteRenderer m_ItemSprite;
        private ItemBase m_ItemBase;
        public ItemBase HandledItem => m_ItemBase;



        public void InitiateItem(ItemBase item)
        {
            if (item == null) { Debug.LogError("ItemBase isn't assigned to the spawning parent!");return; }
            m_ItemBase = item;

            if (m_ItemBase.Icon == null) { Debug.LogError("Sprite isn't set up on the ItemBase prefab: " + m_ItemBase.name); return; }
            m_ItemSprite.sprite = m_ItemBase.Icon;
            
        }

    }
}
