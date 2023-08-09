using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScorgedEarth
{
    [CreateAssetMenu]
    public class TileBehaviourRule : ScriptableObject
    {
        public string GroupName;
        public TileGroup[] TileGroups;
    }
}
