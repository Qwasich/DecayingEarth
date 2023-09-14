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
        /// X размер генерируемой пещеры
        /// </summary>
        public int m_CaveXSize;
        /// <summary>
        /// Y размер генерируемой пещеры
        /// </summary>
        public int m_CaveYSize;

        /// <summary>
        /// Процент заполнения карты при первой генерации. Оптимально - 52-56%
        /// </summary>
        [Header("Generation Settings")]
        [Range(0, 100)] public int m_MapFillPercent;

        /// <summary>
        /// Количечство итераций через клеточный автомат. Больше итераций - более "гладкие" стены.
        /// </summary>
        public int m_NumberOfCAIterations;

        /// <summary>
        /// Список генерируемой руды. Можно как и повысить шанс непосредственно на объекте, можно и добавить руду в список несколько раз
        /// </summary>
        [Header("Ore Generation")]
        public TileGroup_Ore[] Ores;
        /// <summary>
        /// Дополнительный множитель, увеличивающий количество возможной руды на карте.
        /// </summary>
        public int OreGenerationMultiplier;
    }
}
