using UnityEngine;
using Utility;

namespace DecayingEarth
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
        public bool IsTop => Instance.m_IsTop;

        private bool m_IsInventoryHidden = true;
        /// <summary>
        /// ��������, ����� �� ��������� ��� ���
        /// </summary>
        public bool IsInventoryHidden => Instance.m_IsInventoryHidden;

        private bool m_IsLastClickWasOnCanvas = false;
        /// <summary>
        /// ���� �� ��������� ������� �� ����� ������� Canvas
        /// </summary>
        public bool IsLastClickWasOnCanvas => m_IsLastClickWasOnCanvas;

        private bool m_IsMainMenuOpen = false;
        /// <summary>
        /// ������� �� ������� ����
        /// </summary>
        public bool IsMainMenuOpen => m_IsMainMenuOpen;



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

        /// <summary>
        /// ��������� ������� ���� ��������� ���������
        /// </summary>
        /// <param name="v">�������� ���������</param>
        public void UpdateInventoryVisibility(bool v) => m_IsInventoryHidden = v;

        /// <summary>
        /// ��������� ���� �� ��������� ������� ���� �� ��������� ��� ���
        /// </summary>
        /// <param name="v">��������</param>
        public void UpdateLastClick(bool v) => m_IsLastClickWasOnCanvas = v;

        /// <summary>
        /// ��������� ������� �� ���� ��� ���
        /// </summary>
        /// <param name="v">��������</param>
        public void UpdateMainMenu(bool v) => m_IsMainMenuOpen = v;


    }
}
