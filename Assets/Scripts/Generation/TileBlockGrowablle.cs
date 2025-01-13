using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DecayingEarth
{
    public interface IGrowable
    {
        public void PassInfo();
    }

    public class TileBlockGrowablle : TileBlockBase, IGrowable
    {
        [Header("Grow Settings")]
        [SerializeField] private TileBlockBase m_NextBlock;
        /// <summary>
        /// Тайл, в который "вырастет" данный тайл
        /// </summary>
        public TileBlockBase NextBlock => m_NextBlock;

        [SerializeField] private int m_GrowthChance;
        /// <summary>
        /// Шанс роста, рассчитывается как 1 деленное на данное число, раз в заданное количество секунд на отслеживающем синглтоне.
        /// Очевидно, чем данное число выше - тем меньше шанс.
        /// </summary>
        public int GrowthChance => m_GrowthChance;

        [SerializeField] private bool m_CanSpread = false;
        /// <summary>
        /// Может ли блок распространятся самостоятельно
        /// </summary>
        public bool CanSpread => m_CanSpread;

        [SerializeField] private TileBlockBase m_SpreadableBlock;
        /// <summary>
        /// Блок, который данный тайл распространяет
        /// </summary>
        public TileBlockBase SpreadableBlock => m_SpreadableBlock;


        public void PassInfo()
        {
            Debug.Log("Found");
        }


        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (m_GrowthChance < 1) m_GrowthChance = 1;
            if (m_CanSpread && m_SpreadableBlock == null) m_CanSpread = false;
            if (Singleton_CropManager.Instance != null && m_NextBlock != null)
            {
                Singleton_CropManager.Instance.AddCropToList(this, position);
            }
            return base.StartUp(position, tilemap, go);
        }

        public override int DealDamage(int damage, Vector3 pos, Vector3Int tilePosition)
        {
            if (m_IsIndestructible) return 2;
            if (damage * 5 <= m_MaxDurability) return 2;

            Singleton_CropManager.Instance.RemoveCropFromList(tilePosition);

            m_RemainingDurability -= damage;

            if (m_RemainingDurability <= 0)
            {
                if (m_Loot == null) return 0;
                for (int i = 0; i < m_Loot.Length; i++)
                {
                    if (m_Loot[i] == null) continue;
                    GameObject m = Instantiate(Singleton_PrefabLibrary.Instance.DummyItemPrefab);
                    m.GetComponent<PhysicalItem>().InitiateItem(m_Loot[i]);
                    m.transform.position = pos + new Vector3(0.25f, 0.25f);
                }
                m_RemainingDurability = m_MaxDurability;
                return 0;
            }
            else return 1;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/Custom Tiles/Growing Tile")]
        new public static void CreateCustomTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Block Tile", "New Block Tile", "asset", "Save Block Tile", "Assets/Sprites");
            if (path == "") return;

            AssetDatabase.CreateAsset(CreateInstance<TileBlockGrowablle>(), path);
        }
#endif
    }
}
