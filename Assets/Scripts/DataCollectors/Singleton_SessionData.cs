using UnityEngine;
using Utility;

namespace ScorgedEarth
{
    public class Singleton_SessionData : MonoSingleton<Singleton_SessionData>
    {
        private Vector2Int m_LastTileInteractionCoordinate = Vector2Int.zero;
        /// <summary>
        /// Последняя координата тайла, с которой происходило взаимодействие
        /// </summary>
        public Vector2Int LastTileCoordinate => m_LastTileInteractionCoordinate;

        private bool m_IsTop = false;
        /// <summary>
        /// Где новая координата находится в плане высоты относительно предыдущей. Если новая выше - возвращает true.
        /// </summary>
        public bool IsTop => Instance.m_IsTop;

        private bool m_IsInventoryHidden = true;
        public bool IsInventoryHidden => Instance.m_IsInventoryHidden;


        /// <summary>
        /// Обновить последнюю координату взаимодействия.
        /// </summary>
        /// <param name="lastcoordinate">Координата</param>
        /// <returns></returns>
        public void UpdateLastTileCoordinate(Vector2Int lastcoordinate)
        {
            if (lastcoordinate.y < m_LastTileInteractionCoordinate.y) m_IsTop = false;
            if (lastcoordinate.y > m_LastTileInteractionCoordinate.y) m_IsTop = true;
             m_LastTileInteractionCoordinate = lastcoordinate;
        }

        /// <summary>
        /// Обновляет видимую булю видимости инвентаря
        /// </summary>
        /// <param name="v">Значение видимости</param>
        /// <returns></returns>
        public bool UpdateInventoryVisibility(bool v) => m_IsInventoryHidden = v;

    }
}
