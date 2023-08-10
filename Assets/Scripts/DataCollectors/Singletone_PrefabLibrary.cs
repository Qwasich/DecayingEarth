using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ScorgedEarth
{
    public class Singletone_PrefabLibrary : MonoSingleton<Singletone_PrefabLibrary>
    {
        [SerializeField] private  GameObject m_DummyItemPrefab;
        public static GameObject DummyItemPrefab => Instance.m_DummyItemPrefab;
    }
}
