using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class ItemProjectile : ItemBase, UseItem
    {
        protected override ItemType m_ItemType => ItemType.Ammo;

        [SerializeField] protected int m_DealtDamage = 1;
        /// <summary>
        /// Наносинсый снарядом урон при попадании.
        /// </summary>
        public int DealtDamage => m_DealtDamage;

        [SerializeField] private AmmoType m_AmmoType = AmmoType.Arrow;
        /// <summary>
        /// Тип снаряда
        /// </summary>
        public AmmoType AmmoType => m_AmmoType;

        [SerializeField] private float m_Velocity;
        /// <summary>
        /// Скорость объекта
        /// </summary>
        public float Velocity => m_Velocity;

        [SerializeField] private bool m_IsDroppableOnCollision;
        /// <summary>
        /// Будет ли предмет выпадать при его уничтожении
        /// </summary>
        public bool IsDroppableOnCollision => m_IsDroppableOnCollision;

        [SerializeField] private DamageType m_DamageType = DamageType.Ranged;
        /// <summary>
        /// Тип урона, наносимый снарядом
        /// </summary>
        public DamageType DamageType => m_DamageType;

        [SerializeField] private float m_MaxLifetime;
        /// <summary>
        /// Таймер существование снаряда, при нуле - он уничтожается
        /// </summary>
        public float MaxLifetime => m_MaxLifetime;

        public int UseItem(int clickType, Creature creature, InvEntryPoint inventory)
        {
            return 0;
        }
    }
}
