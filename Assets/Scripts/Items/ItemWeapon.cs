using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class ItemWeapon : ItemBase, UseItem
    {

        
        protected override ItemType m_ItemType => ItemType.Weapon;

        [Header("Weapon Attributes")]

        [SerializeField] protected DamageType m_DamageType = DamageType.Melee;
        /// <summary>
        /// ��������� ������� ��� �����
        /// </summary>
        public DamageType DamageType => m_DamageType;

        [SerializeField] protected int m_DealtDamage = 1;
        /// <summary>
        /// ���������� ����� ������ ���� ��� ���������.
        /// </summary>
        public int DealtDamage => m_DealtDamage;    
        [SerializeField] protected bool m_DoesWeaponSpriteDealsDamage = true;
        /// <summary>
        /// ���� True, ���� ������� ����� �������� ���� ��� ������������
        /// </summary>
        public bool DoesWeaponSpriteDealsDamage => m_DoesWeaponSpriteDealsDamage;


        [SerializeField] protected int m_Knockback = 1;
        /// <summary>
        /// 
        /// </summary>
        public int Knockback => m_Knockback;
        //TODO: �������� ����������� ��������� ������ ��� �������������
        //�������� ����������� ��������� ����������� ������ ������, ��� ��������� ���������.

        public int UseItem(int clickType)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return 0;

            return StackDecreaseAfterUse;
        }
    }
}
