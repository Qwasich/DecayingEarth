using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class ItemTool : ItemWeapon, UseItem
    {
        [Header("Mining Attributes")]
        [SerializeField] private int m_MiningDamage = 1;
        /// <summary>
        /// Сколько урона наносится тайлу за одно взаимодействие
        /// </summary>
        public int MiningDamage => m_MiningDamage;


        [SerializeField] private float m_MaxDistance = 3f;
        /// <summary>
        /// Максимальная дистанция урона для кирки
        /// </summary>
        public float MaxDistance => m_MaxDistance;

        new public int UseItem(int clickType, Creature creature, InvEntryPoint inventory)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return 0;

            if (clickType == 0)
            {
                if (!Singleton_SessionData.Instance.IsInventoryHidden) return 0;

                if (m_UsesAmmo && m_SelfProjectile != null && inventory != null)
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


                bool b = Singleton_BlockEditor.Instance.DamageWallBlock(m_MiningDamage, m_MaxDistance);

                if (b) return StackDecreaseAfterUse;
                else return 0;

            }


            return 0;
        }
    }
}
