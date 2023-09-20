using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DecayingEarth
{
    public class TileBlockStorageChest : TileBlockBase
    {

        /// <summary>
        /// Возвращает 0 если тайл уничтожен, 1 если повреждения прошли, 2 если блок неразрушим или слишком маленький урон.
        /// </summary>
        /// <param name="damage">Наносимый урон</param>
        /// <param name="pos">Позиция справна предмета</param>
        /// <returns></returns>
        public override int DealDamage(int damage, Vector3 pos, Vector3Int tilePosition)
        {
            if (m_IsIndestructible) return 2;
            if (damage * 5 <= m_MaxDurability) return 2;

            if (!Singleton_GlobalChestController.Instance.CheckInventoryEmpriness(tilePosition)) return 2;

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
                Singleton_GlobalChestController.Instance.RemoveInventory(tilePosition);
                return 0;
            }
            else return 1;
        }


#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/Custom Tiles/Storage Block Tile")]
        new public static void CreateCustomTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Block Tile", "New Block Tile", "asset", "Save Block Tile", "Assets/Sprites");
            if (path == "") return;

            AssetDatabase.CreateAsset(CreateInstance<TileBlockStorageChest>(), path);
        }
#endif
    }
}
