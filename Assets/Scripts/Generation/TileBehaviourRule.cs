using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScourgedEarth
{
    [CreateAssetMenu]
    public class TileBehaviourRule : ScriptableObject
    {
        public string GroupName;
        public string Tag;
        public TileGroup[] TileGroups;
    }
}
