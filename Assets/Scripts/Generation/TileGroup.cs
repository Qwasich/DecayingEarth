using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScourgedEarth
{
    [CreateAssetMenu]
    public class TileGroup : ScriptableObject
    {
        public string GroupName;
        public TileBase[] Tiles; 
    }
}
