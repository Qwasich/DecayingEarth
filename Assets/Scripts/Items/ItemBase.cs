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

        protected virtual ItemType m_ItemType { get; }
        public ItemType  ItemType => m_ItemType;

    }
}
