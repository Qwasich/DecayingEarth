using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

namespace DecayingEarth
{
    public class Singletone_GridLibrary : MonoSingleton<Singletone_GridLibrary>
    {
        [SerializeField] private Tilemap m_WallsTilemap;
        [SerializeField] private Tilemap m_FloorTilemap;
        [SerializeField] private Tilemap m_OresTilemap;
        [SerializeField] private Tilemap m_EffectsTilemap;

        public Tilemap WallsTilemap => m_WallsTilemap;
        public Tilemap FloorTilemap => m_FloorTilemap;
        public Tilemap OresTilemap => m_OresTilemap;
        public Tilemap EffectsTilemap => m_EffectsTilemap;
    }
}
