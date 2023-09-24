using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace DecayingEarth
{
    public class Singleton_PrefabLibrary : MonoSingleton<Singleton_PrefabLibrary>
    {
        [SerializeField] private GameObject m_DummyItemPrefab;
        /// <summary>
        /// ����������� ������ ��������.
        /// </summary>
        public GameObject DummyItemPrefab => m_DummyItemPrefab;

        [SerializeField] private Sprite m_EmptySprite;
        /// <summary>
        /// ����������� ������ ������ (Null ������������� ����� �������)
        /// </summary>
        public Sprite EmptySprite => m_EmptySprite;

        [SerializeField] private GameObject m_CubeParticles;
        /// <summary>
        /// ����������� ������� ������ ��� ����������� ������
        /// </summary>
        public GameObject CubeParticles => m_CubeParticles;

        [SerializeField] private GameObject m_LightSourcePrefab;
        /// <summary>
        /// ������� ������ ��������� �����
        /// </summary>
        public GameObject LightSourcePrefab => m_LightSourcePrefab;

        [SerializeField] private TileBlockBase m_FillerTile;
        /// <summary>
        /// ��������� ����, ������������ ��� ���������� ������������ ��������. �������, ������, �.�. ������� ������ ���������� ������� ���.
        /// </summary>
        public TileBlockBase FillerTile => m_FillerTile;

        [SerializeField] private GameObject m_BigRecipeHolder;
        /// <summary>
        /// ������� ������ ��� ������ �������, ������
        /// </summary>
        public GameObject BigRecipeHolder => m_BigRecipeHolder;

        [SerializeField] private GameObject m_RecipeButton;
        /// <summary>
        /// ������ ��� ��������
        /// </summary>
        public GameObject RecipeButton => m_RecipeButton;

        [SerializeField] private GameObject m_ProjectileDummy;
        /// <summary>
        /// ������� ������ �������
        /// </summary>
        public GameObject ProjectileDummy => m_ProjectileDummy;


    }
}
