using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class WorldGenerationRules : ScriptableObject
    {
        [Header("Generation Rules")]
        public string m_WorldName;
        [Header("Main World Tiles")]
        public TileBehaviourRule[] m_FloorRule;
        public TileBehaviourRule[] m_WallFrontRule;
        public TileBehaviourRule[] m_WallTopRule;

        /// <summary>
        /// X ������ ������������ ������
        /// </summary>
        public int m_CaveXSize;
        /// <summary>
        /// Y ������ ������������ ������
        /// </summary>
        public int m_CaveYSize;

        /// <summary>
        /// ������� ���������� ����� ��� ������ ���������. ���������� - 52-56%
        /// </summary>
        [Header("Generation Settings")]
        [Range(0, 100)] public int m_MapFillPercent;

        /// <summary>
        /// ����������� �������� ����� ��������� �������. ������ �������� - ����� "�������" �����.
        /// </summary>
        public int m_NumberOfCAIterations;

        /// <summary>
        /// ������ ������������ ����. ����� ��� � �������� ���� ��������������� �� �������, ����� � �������� ���� � ������ ��������� ���
        /// </summary>
        [Header("Ore Generation")]
        public TileGroup_Ore[] Ores;
        /// <summary>
        /// �������������� ���������, ������������� ���������� ��������� ���� �� �����.
        /// </summary>
        public int OreGenerationMultiplier;
    }
}
