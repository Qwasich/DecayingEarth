using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class ItemProjectile : ItemBase, UseItem
    {
        protected override ItemType m_ItemType => ItemType.Ammo;

        [SerializeField] protected int m_DealtDamage = 1;
        /// <summary>
        /// ���������� �������� ���� ��� ���������.
        /// </summary>
        public int DealtDamage => m_DealtDamage;

        [SerializeField] private AmmoType m_AmmoType = AmmoType.Arrow;
        /// <summary>
        /// ��� �������
        /// </summary>
        public AmmoType AmmoType => m_AmmoType;

        [SerializeField] private float m_Velocity;
        /// <summary>
        /// �������� �������
        /// </summary>
        public float Velocity => m_Velocity;

        [SerializeField] private bool m_IsDroppableOnCollision;
        /// <summary>
        /// ����� �� ������� �������� ��� ��� �����������
        /// </summary>
        public bool IsDroppableOnCollision => m_IsDroppableOnCollision;

        [SerializeField] private DamageType m_DamageType = DamageType.Ranged;
        /// <summary>
        /// ��� �����, ��������� ��������
        /// </summary>
        public DamageType DamageType => m_DamageType;

        [SerializeField] private float m_MaxLifetime;
        /// <summary>
        /// ������ ������������� �������, ��� ���� - �� ������������
        /// </summary>
        public float MaxLifetime => m_MaxLifetime;

        public int UseItem(int clickType, Creature creature, InvEntryPoint inventory)
        {
            return 0;
        }
    }
}
