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

        [SerializeField] private float m_MiningSpeed = 1f;
        /// <summary>
        /// Раз в какое время (в секундах) кирка наносит урон тайлу.
        /// </summary>
        public float MiningSpeed => m_MiningSpeed;

        [SerializeField] private float m_MaxDistance = 3f;
        /// <summary>
        /// Максимальная дистанция урона для кирки
        /// </summary>
        public float MaxDistance => m_MaxDistance;

        new public int UseItem(int clickType)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return 0;

            if (clickType == 0)
            {
                Singleton_BlockBreaker.Instance.DamageWallBlock(m_MiningDamage, m_MaxDistance);
                return StackDecreaseAfterUse;
            }


            return 0;
        }
    }
}
