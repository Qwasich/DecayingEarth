using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace DecayingEarth
{
    public class Singleton_TileLibrary : MonoSingleton<Singleton_TileLibrary>
    {
        [SerializeField] private List<TileBehaviourRule> m_FloorLibrary;
        [SerializeField] private List<TileBehaviourRule> m_SideWallLibrary;
        [SerializeField] private List<TileBehaviourRule> m_TopWallLibrary;
        [SerializeField] private List<TileBehaviourRule> m_FloorFeatureLibrary;

        /// <summary>
        /// ��������� ������� ���� �� ����. ���� �� ������� - ���������� null
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>���������� ������� ���� ����� �������, ����� ������ Null</returns>
        public TileBehaviourRule ReturnFloorRuleByTag(string tag)
        {
            for (int i = 0; i < m_FloorLibrary.Count; i++)
            {
                if (m_FloorLibrary[i].Tag == tag) return m_FloorLibrary[i];
            }
            return null;
        }
        /// <summary>
        /// ��������� ������� �������� ����� �� ����. ���� �� ������� - ���������� null
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>���������� ������� ���� ����� �������, ����� ������ Null</returns>
        public TileBehaviourRule ReturnWallSideRuleByTag(string tag)
        {
            for (int i = 0; i < m_SideWallLibrary.Count; i++)
            {
                if (m_SideWallLibrary[i].Tag == tag) return m_SideWallLibrary[i];
            }
            return null;
        }
        /// <summary>
        /// ��������� ������� ������� ����� �� ����. ���� �� ������� - ���������� null
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>���������� ������� ���� ����� �������, ����� ������ Null</returns>
        public TileBehaviourRule ReturnWallTopRuleByTag(string tag)
        {
            for (int i = 0; i < m_TopWallLibrary.Count; i++)
            {
                if (m_TopWallLibrary[i].Tag == tag) return m_TopWallLibrary[i];
            }
            return null;
        }

        /// <summary>
        /// ���������� ������� ���� ���� �� ����. ���� �� ������� - ���������� Null
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>���������� ������� ���� ����� �������, ����� ������ Null</returns>
        public TileBehaviourRule ReturnFloorFeatureRuleByTag(string tag)
        {
            for (int i = 0; i < m_FloorFeatureLibrary.Count; i++)
            {
                if (m_FloorFeatureLibrary[i].Tag == tag) return m_FloorFeatureLibrary[i];
            }
            return null;
        }
    }
}
