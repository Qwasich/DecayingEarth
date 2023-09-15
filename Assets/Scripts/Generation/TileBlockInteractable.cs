using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif



namespace DecayingEarth
{
    public class TileBlockInteractable : TileBlockBase
    {

        

        




#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/Custom Tiles/Interactable Block Tile")]
        new public static void CreateCustomTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Block Tile", "New Block Tile", "asset", "Save Block Tile", "Assets/Sprites");
            if (path == "") return;

            AssetDatabase.CreateAsset(CreateInstance<TileBlockInteractable>(), path);
        }
#endif
    }
}
