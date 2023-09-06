using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class TileGroup_Ore : TileGroup
    {
        /// <summary>
        /// Количество гарантированных жил на карте, минимальное значение.
        /// </summary>
        public int RarityMin;
        /// <summary>
        /// Количество гарантированных жил на карте, максимальное значение значение.
        /// </summary>
        public int RarityMax;
        /// <summary>
        /// Максимальное количество руды в одной жиле.
        /// </summary>
        public int VeinSize;
        


    }
}
