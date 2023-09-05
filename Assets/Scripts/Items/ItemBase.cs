using UnityEngine;

namespace ScourgedEarth
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

    public enum DamageType
    {
        Melee,
        Ranged,
        Magic
    }

    
    public enum HoldType
    {
        Swing,
        Pierce,
        Static,
        Empty
    }

    public interface UseItem
    {
        /// <summary>
        /// ���������� �������, � ����������� �� ������� ������� ����
        /// </summary>
        /// <param name="clickType">0 - �����, 1 - ������.</param>
        /// <returns>���������� �����, �� ������� ���� ��������� ���� ����� ��������� �������������. ����� ���������� ����.</returns>
        public int UseItem(int clickType);
    }

    public abstract class ItemBase : ScriptableObject
    {
        [Header("Item Basics")]
        public string Name;
        public Sprite Icon;
        [SerializeField] protected int m_MaxItemCount = 1;
        public int MaxStackCount => m_MaxItemCount;

        protected virtual ItemType m_ItemType { get; }
        public ItemType  ItemType => m_ItemType;

        [SerializeField] protected float m_UseTimer = 1f;
        /// <summary>
        /// ������ ���������� ���������� ������������� ��������
        /// </summary>
        public float UseTimer => m_UseTimer;

        [SerializeField] protected string m_Tooltip = "";
        /// <summary>
        /// �������������� �����, �������������� � ��������. ������� ����� ��� �������� ��� �����-������ �����.
        /// </summary>
        public string Tooltip => m_Tooltip;

        [SerializeField] protected int StackDecreaseAfterUse = 0;
    }
}
