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
        private TileBehaviourRule m_WallFrontRule;
        private TileBehaviourRule m_WallTopRule;
        [SerializeField] private int m_Damage;
        [SerializeField] private float m_MaximimDistance;
        [SerializeField] private int m_Radius = 0;
        TileBlockBase m_Tile;

        private void Start()
        {
            m_WallsTilemap = Cave_Generator.Instance.WallsTilemap;
            m_OresTilemap = Cave_Generator.Instance.OresTilemap;
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
                int i = tile.DealDamage(m_Damage);
                if (i == 0)
                {
                    m_WallsTilemap.SetTile(coordinate, null);
                    WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, m_WallFrontRule, m_WallTopRule, tile, m_Radius, false);
                    
                }
                    

            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float dist = Vector2.Distance(transform.position, pos);
                if (dist > m_MaximimDistance) { Debug.Log("Position:" + dist); return; }
                if (dist < 1) { Debug.Log("Position:" + dist); return; }
                Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
                TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
                if (tile != null && tile.BlockType == BlockType.TOP) return;
                
                m_WallsTilemap.SetTile(coordinate, m_WallTopRule.TileGroups[0].Tiles[0]);
                if (tile == null) tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
                Debug.Log(coordinate);

                WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, m_WallFrontRule, m_WallTopRule, tile, m_Radius,true);




            }

        }
    }
}
