using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ScorgedEarth
{
    public enum BlockType
    {
        FLOOR,
        SIDE,
        TOP,
        EFFECT
    }

    public class TileBlockBase : Tile
    {
        /// <summary>
        /// Тип блока
        /// </summary>
        [SerializeField] private BlockType m_BlockType = BlockType.TOP;
        public BlockType BlockType => m_BlockType;

        [SerializeField] private string m_Tag;
        /// <summary>
        /// Тэг блока
        /// </summary>
        public string Tag => m_Tag;
        /// <summary>
        /// Прочность блока.
        /// </summary>
        [SerializeField] private int m_Durability;

        private int m_MaxDurability;
        public int MaxDurability => m_MaxDurability;

        private int m_RemainingDurability;
        public int RemainingDurability => m_RemainingDurability;

        /// <summary>
        /// Если истинно, любой урон игнорируется.
        /// </summary>
        [SerializeField] private bool m_IsIndestructible;

        /// <summary>
        /// Предмет, выпадающий при уничтожении.
        /// </summary>
        [SerializeField] private GameObject m_Loot;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            m_MaxDurability = m_Durability;
            m_RemainingDurability = m_Durability;
            return base.StartUp(position, tilemap, go); 
        }

        /// <summary>
        /// Возвращает 0 если тайл уничтожен, 1 если повреждения прошли, 2 если блок неразрушим или слишком маленький урон.
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public int DealDamage(int damage)
        {
            //Debug.Log(m_RemainingDurability + " " + damage);
            if (m_IsIndestructible) return 2;
            if (damage * 5 <= m_MaxDurability) return 2;

            m_RemainingDurability -= damage;
            
            if (m_RemainingDurability <= 0)
            {
                if (m_Loot != null) Instantiate(m_Loot, this.transform.GetPosition(), transform.rotation);
                m_RemainingDurability = m_MaxDurability;
                return 0;
            }
            else return 1;
        }

        public void Recover() => m_RemainingDurability = m_MaxDurability;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/Custom Tiles/Block Tile")]
        public static void CreateCustomTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Block Tile", "New Block Tile", "asset", "Save Block Tile","Assets/Sprites");
            if (path == "") return;

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileBlockBase>(), path);
        }
#endif

    }
}
