using UnityEngine;
using Utility;

namespace ScorgedEarth
{
    public class Singleton_SessionData : MonoSingleton<Singleton_SessionData>
    {
        private Vector2Int m_LastTileInteractionCoordinate = Vector2Int.zero;
        /// <summary>
        /// ��������� ���������� �����, � ������� ����������� ��������������
        /// </summary>
        public Vector2Int LastTileCoordinate => m_LastTileInteractionCoordinate;

        private bool m_IsTop = false;
        /// <summary>
        /// ��� ����� ���������� ��������� � ����� ������ ������������ ����������. ���� ����� ���� - ���������� true.
        /// </summary>
        public static bool IsTop => Instance.m_IsTop;


        /// <summary>
        /// �������� ��������� ���������� ��������������.
        /// </summary>
        /// <param name="lastcoordinate">����������</param>
        /// <returns></returns>
        public void UpdateLastTileCoordinate(Vector2Int lastcoordinate)
        {
            if (lastcoordinate.y < m_LastTileInteractionCoordinate.y) m_IsTop = false;
            if (lastcoordinate.y > m_LastTileInteractionCoordinate.y) m_IsTop = true;
             m_LastTileInteractionCoordinate = lastcoordinate;
        }

    }
}
