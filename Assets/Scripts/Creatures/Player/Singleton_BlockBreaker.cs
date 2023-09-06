using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

namespace DecayingEarth
{
    public class Singleton_BlockBreaker : MonoSingleton<Singleton_BlockBreaker>
    {
        private Tilemap m_WallsTilemap;
        private Tilemap m_OresTilemap;
        private Tilemap m_FloorTilemap;
        private TileBehaviourRule m_WallFrontRule;
        private TileBehaviourRule m_WallTopRule;

        private void Start()
        {
            m_WallsTilemap = Cave_Generator.Instance.WallsTilemap;
            m_OresTilemap = Cave_Generator.Instance.OresTilemap;
            m_FloorTilemap = Cave_Generator.Instance.FloorTilemap;
            m_WallFrontRule = Cave_Generator.Instance.WallFrontRule;
            m_WallTopRule = Cave_Generator.Instance.WallTopRule;
        }


        /// <summary>
        /// Повреждает блок
        /// </summary>
        /// <param name="damage">Урон, наносимый блоку</param>
        /// <param name="maxDistance">Максимальная дистанция от игрока для нанесения урона</param>
        /// <param name="radius">Радиус редактирования стен</param>
        /// <returns>True если изменение успешно</returns>
        public bool DamageWallBlock(int damage, float maxDistance = 3, int radius = 1)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return false;

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float dist = Vector2.Distance(Camera.main.transform.position, pos);
            if (dist > maxDistance) { Debug.Log("Position:" + dist); return false; }
            Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
            TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            if (tile == null) return false;
            TileBlockBase ore = null;
            if (tile.BlockType == BlockType.TOP) ore = m_OresTilemap.GetTile<TileBlockBase>(coordinate);
            else if (tile.BlockType == BlockType.SIDE) ore = m_OresTilemap.GetTile<TileBlockBase>(new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z));

            if (Singleton_SessionData.Instance.LastTileCoordinate != (Vector2Int)coordinate)
            {
                tile.Recover();
                if (ore != null) ore.Recover();
            }
            Singleton_SessionData.Instance.UpdateLastTileCoordinate((Vector2Int)coordinate);

            int i = 1;
            if (ore == null) i = tile.DealDamage(damage, m_WallsTilemap.CellToWorld(coordinate));
            else i = ore.DealDamage(damage, m_WallsTilemap.CellToWorld(coordinate));

            if (i == 0)
            {
                if (ore != null && tile.BlockType == BlockType.TOP) m_OresTilemap.SetTile(coordinate, null);
                else if (ore != null && tile.BlockType == BlockType.SIDE) m_OresTilemap.SetTile(new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z), null);
                if (ore != null) tile.DealDamage(1000000, m_WallsTilemap.CellToWorld(coordinate));
                m_WallsTilemap.SetTile(coordinate, null);
                WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, tile, radius, false);
            }

            return true;
        }

        /// <summary>
        /// Ставит блок, указаный в предмете
        /// </summary>
        /// <param name="itemTile">Устанавливаемый тайл</param>
        /// <param name="maxDistance">Максимальная дистанция от игрока для нанесения урона</param>
        /// <param name="radius">Радиус редактирования стен</param>
        ///<returns>True если изменение успешно</returns>
        public bool PlaceWallBlock(TileBlockBase itemTile, float maxDistance = 3, int radius = 1)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return false;

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (CheckRigidbody(pos) == true) return false;
            float dist = Vector2.Distance(Camera.main.transform.position, pos);
            if (dist > maxDistance) { Debug.Log("Position:" + dist); return false; }
            Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
            TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            if (tile != null && tile.BlockType == BlockType.TOP) return false;

            m_WallsTilemap.SetTile(coordinate, itemTile);
            if (tile == null) tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            //Debug.Log(coordinate);

            WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, tile, radius, true);

            return true;
        }



        private bool CheckRigidbody(Vector2 pos)
        {
            RaycastHit2D[] hitdata;
            hitdata = Physics2D.BoxCastAll(pos,new Vector2(0.75f, 0.75f),0, Vector3.forward);
            foreach(RaycastHit2D rh in hitdata)
            {
                if (rh.collider.gameObject.name == "PickupHitbox") continue;
                Rigidbody2D rb = rh.rigidbody;
                if (rb != null)
                {
                    Debug.Log("rb is discovered on object: " + rb.name);
                    return true;
                }
                
            }

            return false;
        }
    }
}
