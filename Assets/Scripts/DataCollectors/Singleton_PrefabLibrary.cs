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
        /// ����������� ������ ��������.
        /// </summary>
        public static GameObject DummyItemPrefab => Instance.m_DummyItemPrefab;

        [SerializeField] private Sprite m_EmptySprite;
        /// <summary>
        /// ����������� ������ ������ (Null ������������� ����� �������)
        /// </summary>
        public static Sprite EmptySprite => Instance.m_EmptySprite;
    }
}
