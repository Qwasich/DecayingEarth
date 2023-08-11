using UnityEngine;

namespace ScorgedEarth
{
    public enum ItemType
    {
        Material,
        Block,
        Weapon,
        Tool,
        ArmorHead,
        ArmorBody,
        ArmorLegs,
        Accessory
    }

    public abstract class ItemBase : ScriptableObject
    {
        [Header("Item Basics")]
        public string Name;
        public Sprite Icon;
        [SerializeField] private int m_MaxItemCount = 1;
        public int MaxStackCount => m_MaxItemCount;

        protected virtual ItemType m_ItemType { get; }
        public ItemType  ItemType => m_ItemType;

    }
}
