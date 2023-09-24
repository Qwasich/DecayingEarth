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

        [Header("Projectile Settings")]
        [SerializeField] protected bool m_UsesAmmo = false;
        /// <summary>
        /// ���� True, ������ ����� �������� �������������� ���� ���������
        /// </summary>
        public bool UsesAmmo => m_UsesAmmo;

        [SerializeField] protected ItemProjectile m_SelfProjectile;
        /// <summary>
        /// ������, ������� ����� ��������� ������. ���� ������� ����� ���������, ����� ������ �������� ���� �������� (���� �� ��������)
        /// ��� �� ������ ��� �������� ���� ��������, ������� ����� ������������ ������ ������
        /// </summary>
        public ItemProjectile Projectile => m_SelfProjectile;

        //TODO: 
        //�������� ����������� ��������� ����������� ������ ������, ��� ��������� ���������.

        public int UseItem(int clickType, Creature creature, InvEntryPoint inventory)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return 0;

            if (clickType == 0)
            {
                if (!Singleton_SessionData.Instance.IsInventoryHidden) return 0;

                if(m_UsesAmmo &&  m_SelfProjectile != null && inventory != null)
                {
                    
                    ItemProjectile ipr = CheckForAmmo(inventory);
                    if (ipr != null)
                    {
                        GameObject go = Instantiate(Singleton_PrefabLibrary.Instance.ProjectileDummy);
                        Projectile proj = go.GetComponent<Projectile>();
                        proj.SetParentShooter(creature);
                        proj.Initialize(ipr, m_DealtDamage);
                        WeaponCollisionHandler wch = creature.GetComponentInChildren<WeaponCollisionHandler>();
                        proj.gameObject.transform.position = wch.transform.position;
                        proj.gameObject.transform.eulerAngles = new Vector3(wch.transform.eulerAngles.x, wch.transform.eulerAngles.y, wch.transform.eulerAngles.z - 45);
                    }
                }
                else if (!m_UsesAmmo && m_SelfProjectile != null)
                {
                    GameObject go = Instantiate(Singleton_PrefabLibrary.Instance.ProjectileDummy);
                    Projectile proj = go.GetComponent<Projectile>();
                    proj.SetParentShooter(creature);
                    proj.Initialize(m_SelfProjectile, m_DealtDamage);
                    WeaponCollisionHandler wch = creature.GetComponentInChildren<WeaponCollisionHandler>();
                    proj.gameObject.transform.position = wch.transform.position;
                    proj.gameObject.transform.eulerAngles = new Vector3(wch.transform.eulerAngles.x, wch.transform.eulerAngles.y, wch.transform.eulerAngles.z - 45);
                }
                
            }
            if (clickType == 1)
            {
                //�������� ����������� ��������� ��������� ������� �� ������ ������ ����
                return 0;
            }
            return StackDecreaseAfterUse;
        }

        protected ItemProjectile CheckForAmmo(InvEntryPoint point)
        {
            for(int i = 0; i< point.Inventory.Items.Count; i++)
            {
                if (point.Inventory.Items[i].Item is ItemProjectile && (point.Inventory.Items[i].Item as ItemProjectile).AmmoType == m_SelfProjectile.AmmoType)
                {
                    point.Inventory.DecreaseItemCount(i, 1);
                    point.UpdateButton(i);
                    return (ItemProjectile)point.Inventory.Items[i].Item;
                }
            }
            return null;
        }


    }
}
