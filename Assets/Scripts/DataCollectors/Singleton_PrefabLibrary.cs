using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ScorgedEarth
{
    public class Singleton_PrefabLibrary : MonoSingleton<Singleton_PrefabLibrary>
    {
        [SerializeField] private  GameObject m_DummyItemPrefab;
        /// <summary>
        /// Стандартный префаб предмета.
        /// </summary>
        public GameObject DummyItemPrefab => Instance.m_DummyItemPrefab;

        [SerializeField] private Sprite m_EmptySprite;
        /// <summary>
        /// Стандартный пустой спрайт (Null устанавливает белый квадрат)
        /// </summary>
        public Sprite EmptySprite => Instance.m_EmptySprite;
    }
}
