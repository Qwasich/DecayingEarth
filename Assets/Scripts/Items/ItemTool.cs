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
        /// ������� ����� ��������� ����� �� ���� ��������������
        /// </summary>
        public int MiningDamage => m_MiningDamage;

        [SerializeField] private float m_MiningSpeed = 1f;
        /// <summary>
        /// ��� � ����� ����� (� ��������) ����� ������� ���� �����.
        /// </summary>
        public float MiningSpeed => m_MiningSpeed;

        [SerializeField] private float m_MaxDistance = 3f;
        /// <summary>
        /// ������������ ��������� ����� ��� �����
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
