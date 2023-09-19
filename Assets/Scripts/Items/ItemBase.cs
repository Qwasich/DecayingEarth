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
        /// Использует предмет, в зависимости от нажатой клавиши мыши
        /// </summary>
        /// <param name="clickType">0 - левая, 1 - правая.</param>
        /// <returns>Возвращает число, на которое надо уменьшить стак после успешного использования. Иначе возвращает ноль.</returns>
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
        /// Таймер разрешения повторного использования предмета
        /// </summary>
        public float UseTimer => m_UseTimer;

        [SerializeField] protected string m_Tooltip = "";
        /// <summary>
        /// Дополнительный текст, показывающийся у предмета. Хорошее место для описания или каких-нибудь шуток.
        /// </summary>
        public string Tooltip => m_Tooltip;

        [SerializeField] protected int m_StackDecreaseAfterUse = 0;
        /// <summary>
        /// Сколько предмета будет "израсходовано" после успешного использования.
        /// </summary>
        public int StackDecreaseAfterUse => m_StackDecreaseAfterUse;


        [SerializeField] protected HoldType m_HoldType = HoldType.Empty;
        /// <summary>
        /// Тип, как держиться оружие при использовании - махать или держать перед собой
        /// </summary>
        public HoldType HoldType => m_HoldType;

        [SerializeField] protected float m_SwingAngle = 110f;

        /// <summary>
        /// Если Hold Type = Swing, обозначает угол размаха.
        /// </summary>
        public float SwingAngle => m_SwingAngle;

        [SerializeField] protected float m_ManaPerUse = 0;

        /// <summary>
        /// Количество маны, требуемое для использования предмета.
        /// </summary>
        public float ManaPerUse => m_ManaPerUse;


    }
}
