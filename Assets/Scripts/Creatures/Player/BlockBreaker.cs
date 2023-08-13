using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace ScorgedEarth
{
    public class BlockBreaker : MonoBehaviour
    {
        private Tilemap m_WallsTilemap;
        private Tilemap m_OresTilemap;
        private Tilemap m_FloorTilemap;
        private TileBehaviourRule m_WallFrontRule;
        private TileBehaviourRule m_WallTopRule;
        [SerializeField] private int m_Damage;
        [SerializeField] private float m_MaximimDistance;
        [SerializeField] private float m_MinimumDistance;
        [SerializeField] private int m_Radius = 0;

        private void Start()
        {
            m_WallsTilemap = Cave_Generator.Instance.WallsTilemap;
            m_OresTilemap = Cave_Generator.Instance.OresTilemap;
            m_FloorTilemap = Cave_Generator.Instance.FloorTilemap;
            m_WallFrontRule = Cave_Generator.Instance.WallFrontRule;
            m_WallTopRule = Cave_Generator.Instance.WallTopRule;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float dist = Vector2.Distance(transform.position, pos);
                if (dist > m_MaximimDistance) { Debug.Log("Position:" + dist); return; }
                Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
                TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
                if (tile == null) return;
                TileBlockBase ore = null;
                if (tile.BlockType == BlockType.TOP) ore = m_OresTilemap.GetTile<TileBlockBase>(coordinate);
                else if (tile.BlockType == BlockType.SIDE) ore = m_OresTilemap.GetTile<TileBlockBase>(new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z));
                Singleton_SessionData.Instance.UpdateLastCoordinate((Vector2Int)coordinate);

                int i = 1;
                if (ore == null) i = tile.DealDamage(m_Damage, m_WallsTilemap.CellToWorld(coordinate));
                else i = ore.DealDamage(m_Damage, m_WallsTilemap.CellToWorld(coordinate));

                if (i == 0)
                {
                    if (ore != null && tile.BlockType == BlockType.TOP) m_OresTilemap.SetTile(coordinate, null);
                    else if (ore != null && tile.BlockType == BlockType.SIDE) m_OresTilemap.SetTile(new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z), null);
                    if (ore != null) tile.DealDamage(1000000, m_WallsTilemap.CellToWorld(coordinate));
                    m_WallsTilemap.SetTile(coordinate, null);
                    WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, m_WallFrontRule, m_WallTopRule, tile, m_Radius, false);
                }
                    

            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float dist = Vector2.Distance(transform.position, pos);
                if (dist > m_MaximimDistance) { Debug.Log("Position:" + dist); return; }
                if (dist < m_MinimumDistance) { Debug.Log("Position:" + dist); return; }
                Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
                TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
                if (tile != null && tile.BlockType == BlockType.TOP) return;
                
                m_WallsTilemap.SetTile(coordinate, m_WallTopRule.TileGroups[0].Tiles[0]);
                if (tile == null) tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
                //Debug.Log(coordinate);

                WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, m_WallFrontRule, m_WallTopRule, tile, m_Radius,true);
            }

        }
    }
}
