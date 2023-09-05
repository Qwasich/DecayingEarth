using UnityEngine;

namespace ScourgedEarth
{
    [CreateAssetMenu]
    public class ItemWeapon : ItemBase, UseItem
    {

        
        protected override ItemType m_ItemType => ItemType.Weapon;

        [Header("Weapon Attributes")]

        [SerializeField] protected DamageType m_DamageType = DamageType.Melee;
        /// <summary>
        /// Ќаносимый оружием тип урона
        /// </summary>
        public DamageType DamageType => m_DamageType;

        [SerializeField] protected int m_SwingDamage = 1;
        /// <summary>
        /// Ќаносинсый телом оружи€ урон при попадании.
        /// </summary>
        public int SwingDamage => m_SwingDamage;    
        [SerializeField] protected bool m_DoesWeaponSpriteDealsDamage = true;
        /// <summary>
        /// ≈сли True, тело спрайта будет наносить урон при столкновении
        /// </summary>
        public bool DoesWeaponSpriteDealsDamage => m_DoesWeaponSpriteDealsDamage;

        [SerializeField] protected HoldType m_HoldType = HoldType.Swing;
        /// <summary>
        /// “ип, как держитьс€ оружие при использовании - махать или держать перед собой
        /// </summary>
        public HoldType HoldType => m_HoldType;

        [SerializeField] protected int m_Knockback = 1;
        /// <summary>
        /// 
        /// </summary>
        public int Knockback => m_Knockback;
        //TODO: ƒобавить возможность назначить снар€д при использовании
        //ƒобавить возможность назначать специальный скрипт оружи€, дл€ изменени€ поведени€.

        public int UseItem(int clickType)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return 0;

            return StackDecreaseAfterUse;
        }
    }
}
