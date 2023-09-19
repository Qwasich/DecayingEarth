using UnityEngine;

namespace DecayingEarth
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
        Hold,
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

        [SerializeField] protected int m_StackDecreaseAfterUse = 0;
        /// <summary>
        /// ������� �������� ����� "�������������" ����� ��������� �������������.
        /// </summary>
        public int StackDecreaseAfterUse => m_StackDecreaseAfterUse;


        [SerializeField] protected HoldType m_HoldType = HoldType.Empty;
        /// <summary>
        /// ���, ��� ��������� ������ ��� ������������� - ������ ��� ������� ����� �����
        /// </summary>
        public HoldType HoldType => m_HoldType;

        [SerializeField] protected float m_SwingAngle = 110f;

        /// <summary>
        /// ���� Hold Type = Swing, ���������� ���� �������.
        /// </summary>
        public float SwingAngle => m_SwingAngle;

        [SerializeField] protected float m_ManaPerUse = 0;

        /// <summary>
        /// ���������� ����, ��������� ��� ������������� ��������.
        /// </summary>
        public float ManaPerUse => m_ManaPerUse;


    }
}
