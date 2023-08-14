using UnityEngine;
using Utility;

namespace ScorgedEarth
{
    public class Singleton_SessionData : MonoSingleton<Singleton_SessionData>
    {
        private Vector2Int m_LastInteractionCoordinate = Vector2Int.zero;
        /// <summary>
        /// Последняя координата, с которой происходило взаимодействие
        /// </summary>
        public static Vector2Int LastCoordinate => Instance.m_LastInteractionCoordinate;

        private bool m_IsTop = false;
        /// <summary>
        /// Где новая координата находится в плане высоты относительно предыдущей. Если новая выше - возвращает true.
        /// </summary>
        public static bool IsTop => Instance.m_IsTop;


        /// <summary>
        /// Обновить последнюю координату взаимодействия.
        /// </summary>
        /// <param name="lastcoordinate">Координата</param>
        /// <returns></returns>
        public void UpdateLastCoordinate(Vector2Int lastcoordinate)
        {
            if (lastcoordinate.y < m_LastInteractionCoordinate.y) m_IsTop = false;
            if (lastcoordinate.y > m_LastInteractionCoordinate.y) m_IsTop = true;
             m_LastInteractionCoordinate = lastcoordinate;
        }

    }
}
