using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

namespace DecayingEarth
{
    public class Singleton_GridLibrary : MonoSingleton<Singleton_GridLibrary>
    {
        [SerializeField] private Tilemap m_WallsTilemap;
        [SerializeField] private Tilemap m_FloorTilemap;
        [SerializeField] private Tilemap m_FloorDetsTilemap;
        [SerializeField] private Tilemap m_OresTilemap;
        [SerializeField] private Tilemap m_EffectsTilemap;

        public Tilemap WallsTilemap => m_WallsTilemap;
        public Tilemap FloorTilemap => m_FloorTilemap;
        public Tilemap FloorDetsTilemap => m_FloorDetsTilemap;
        public Tilemap OresTilemap => m_OresTilemap;
        public Tilemap EffectsTilemap => m_EffectsTilemap;
    }
}
