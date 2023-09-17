using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace DecayingEarth
{
    public class Singleton_PrefabLibrary : MonoSingleton<Singleton_PrefabLibrary>
    {
        [SerializeField] private  GameObject m_DummyItemPrefab;
        /// <summary>
        /// Стандартный префаб предмета.
        /// </summary>
        public GameObject DummyItemPrefab => m_DummyItemPrefab;

        [SerializeField] private Sprite m_EmptySprite;
        /// <summary>
        /// Стандартный пустой спрайт (Null устанавливает белый квадрат)
        /// </summary>
        public Sprite EmptySprite => m_EmptySprite;

        [SerializeField] private GameObject m_CubeParticles;
        /// <summary>
        /// Стандартная система частиц для разрушаемых блоков
        /// </summary>
        public GameObject CubeParticles => m_CubeParticles;

        [SerializeField] private GameObject m_LightSourcePrefab;
        /// <summary>
        /// Базовый префаб источника света
        /// </summary>
        public GameObject LightSourcePrefab => m_LightSourcePrefab;

        [SerializeField] private TileBlockBase m_FillerTile;
        /// <summary>
        /// Невидимый тайл, используемый для заполнения многоблочных объектов. Костыль, честно, т.к. времени писать нормальное решение нет.
        /// </summary>
        public TileBlockBase FillerTile => m_FillerTile;


    }
}
