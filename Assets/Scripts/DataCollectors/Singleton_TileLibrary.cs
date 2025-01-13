using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace DecayingEarth
{
    public class Singleton_TileLibrary : MonoSingleton<Singleton_TileLibrary>
    {
        [SerializeField] private TileGroup[] m_TileLibrary;

        /// <summary>
        /// ��������� ������� ���� �� ����. ���� �� ������� - ���������� null
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>���������� ������� ���� ����� �������, ����� ������ Null</returns>
        public TileGroup ReturnTileGroupByTag(string tag)
        {
            for (int i = 0; i < m_TileLibrary.Length; i++)
            {
                if (m_TileLibrary[i].BlockTag == tag) return m_TileLibrary[i];
            }
            return null;
        }
    }
}
