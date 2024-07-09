using System;
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
        public TileGroup[] m_TileRule;

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

        /// <summary>
        /// Генерация особых пещер. 
        /// </summary>
        [Serializable]
        public struct FeatureSlot
        {
            /// <summary>
            /// Правило генерации
            /// </summary>
            public TileGroup TileRule;
            /// <summary>
            /// Минимальное количество фичи для генерации
            /// </summary>
            public int MinimalAmount;
            /// <summary>
            /// Максимальное количество фичи для генерации
            /// </summary>
            public int MaximalAmount;
            /// <summary>
            /// Множитель генерации, увеличивает количество фич.
            /// </summary>
            public int GenerationMultiplier;
        }

        /// <summary>
        /// Генерация особых пещер.
        /// </summary>
        [Header("Features Generation")]
        public FeatureSlot[] m_Features;

        /// <summary>
        /// Массив объектов, который будет сгенерирован картой как "растения"
        /// </summary>
        public TileBlockGrowablle[] m_AllowedFoliage;
        

    }
}
