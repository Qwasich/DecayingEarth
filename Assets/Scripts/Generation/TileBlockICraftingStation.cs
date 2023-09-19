using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif



namespace DecayingEarth
{
    public class TileBlockICraftingStation : TileBlockBase
    {
        [Header("Crafting Recipes")]
        [SerializeField] private string m_WindowName = "";
        /// <summary>
        /// Строка, показывающаяся над окном крафта
        /// </summary>
        public string WindowName => m_WindowName;

        [SerializeField] private List<CraftingRecipeBase> m_Recepies;
        /// <summary>
        /// Лист всех рецептов, привязанных к данному тайлу.
        /// </summary>
        public List<CraftingRecipeBase> Recepies => m_Recepies;







#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/Custom Tiles/Crafting Station Block Tile")]
        new public static void CreateCustomTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Block Tile", "New Block Tile", "asset", "Save Block Tile", "Assets/Sprites");
            if (path == "") return;

            AssetDatabase.CreateAsset(CreateInstance<TileBlockICraftingStation>(), path);
        }
#endif
    }
}
