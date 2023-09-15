using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Random = UnityEngine.Random;

namespace DecayingEarth
{
    public class Singleton_BlockBreaker : MonoSingleton<Singleton_BlockBreaker>
    {
        [SerializeField] private float m_BlockShakeTime = 1f;
        [SerializeField] private float m_BlockShakeIntensity = 1f;
        [SerializeField] private float m_BlockShakesPerSecond = 8f;

        private Tilemap m_WallsTilemap;
        private Tilemap m_OresTilemap;
        private Tilemap m_FloorTilemap;

        private void Start()
        {
            m_WallsTilemap = Singletone_GridLibrary.Instance.WallsTilemap;
            m_OresTilemap = Singletone_GridLibrary.Instance.OresTilemap;
            m_FloorTilemap = Singletone_GridLibrary.Instance.FloorTilemap;
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
            if (dist > maxDistance) return false; 
            TileBlockBase addTile = null;
            Vector3Int addCoord = Vector3Int.zero;
            Vector3Int oreCoord = Vector3Int.zero;

            Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
            TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            if (tile == null) return false;

            if (tile.IsALightSource)
            {
                Collider2D col = CheckTriggerCollider(m_WallsTilemap.CellToWorld(coordinate) + new Vector3(0.25f, 0.25f));

                if (col != null && tile.RemainingDurability == 1) Destroy(col.transform.root.gameObject);
            }

            if (tile.InvokeRule == false)
            {
                int j = 1;
                j = tile.DealDamage(damage, m_WallsTilemap.CellToWorld(coordinate));

                if (j == 0)
                {
                    StopAllCoroutines();
                    m_WallsTilemap.SetTile(coordinate, null);
                }

                return true;
            }

            

            TileBlockBase ore = null;
            if (tile.BlockType == BlockType.TOP)
            {
                ore = m_OresTilemap.GetTile<TileBlockBase>(coordinate);
                oreCoord = new Vector3Int(coordinate.x, coordinate.y, coordinate.z);

                TileBlockBase til = m_WallsTilemap.GetTile<TileBlockBase>(new Vector3Int(coordinate.x, coordinate.y - 1, coordinate.z));
                if (til.BlockType == BlockType.SIDE)
                {
                    addTile = til;
                    addCoord = new Vector3Int(coordinate.x, coordinate.y - 1, coordinate.z);
                    
                }
            }
            else if (tile.BlockType == BlockType.SIDE)
            {
                ore = m_OresTilemap.GetTile<TileBlockBase>(new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z));
                oreCoord = new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z);

                TileBlockBase til = m_WallsTilemap.GetTile<TileBlockBase>(new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z));
                if (til.BlockType == BlockType.TOP)
                {
                    addTile = til;
                    addCoord = new Vector3Int(coordinate.x, coordinate.y + 1, coordinate.z);
                    
                }
            }



            if (Singleton_SessionData.Instance.LastTileCoordinate != (Vector2Int)coordinate)
            {
                tile.Recover();
                if (ore != null) ore.Recover();
            }
            Singleton_SessionData.Instance.UpdateLastTileCoordinate((Vector2Int)coordinate);

            int i = 1;
            if (ore == null) i = tile.DealDamage(damage, m_WallsTilemap.CellToWorld(coordinate));
            else i = ore.DealDamage(damage, m_WallsTilemap.CellToWorld(oreCoord));

            if (i == 0)
            {
                StopAllCoroutines();

                if (tile != null) UsefulBits.FixTilePosition(coordinate, m_WallsTilemap, tile);
                if (ore != null) UsefulBits.FixTilePosition(oreCoord, m_OresTilemap, ore);
                if (addTile != null) UsefulBits.FixTilePosition(addCoord, m_WallsTilemap, addTile);

                if (ore != null)
                {
                    m_OresTilemap.SetTile(oreCoord, null);
                    tile.DealDamage(1000000, m_WallsTilemap.CellToWorld(oreCoord));
                }
                if (addTile != null) ParticleSpawner(m_WallsTilemap.CellToWorld(addCoord), addTile);
                ParticleSpawner(m_WallsTilemap.CellToWorld(coordinate), tile);
                m_WallsTilemap.SetTile(coordinate, null);

                if (tile.BlockType == BlockType.TOP) WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, tile, radius, false);
                if (tile.BlockType == BlockType.SIDE)
                {
                    TileBlockBase adder = m_WallsTilemap.GetTile<TileBlockBase>(new Vector3Int(coordinate.x, coordinate.y + 2, coordinate.z));

                    if (adder == null || adder.BlockType == BlockType.SIDE || adder.InvokeRule == false) WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, tile, radius, false);
                    else WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y + 1, m_WallsTilemap, adder, radius, false);


                }
            }

            else
            {
                int rdseed = Random.Range(0, 500);
                if (tile != null) StartCoroutine(UsefulBits.TileShaker(coordinate, m_WallsTilemap, tile, m_BlockShakeTime, m_BlockShakeIntensity, m_BlockShakesPerSecond,rdseed));
                if (ore != null) StartCoroutine(UsefulBits.TileShaker(oreCoord, m_OresTilemap, ore, m_BlockShakeTime, m_BlockShakeIntensity, m_BlockShakesPerSecond,rdseed));
                if (addTile != null) StartCoroutine(UsefulBits.TileShaker(addCoord, m_WallsTilemap, addTile, m_BlockShakeTime, m_BlockShakeIntensity, m_BlockShakesPerSecond,rdseed));

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
            if (!itemTile.IgnoreRigidbody && CheckRigidbody(pos) == true) return false;
            float dist = Vector2.Distance(Camera.main.transform.position, pos);
            if (dist > maxDistance) { Debug.Log("Position:" + dist); return false; }
            Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
            TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            if (tile != null && (tile.BlockType == BlockType.TOP || itemTile.InvokeRule == false)) return false;

            if (itemTile.IsALightSource)
            {
                
                GameObject ls = Instantiate(Singleton_PrefabLibrary.Instance.LightSourcePrefab);
                ls.transform.position = m_WallsTilemap.CellToWorld(coordinate) + new Vector3(0.25f, 0.25f, -1.34f);
                Light light = ls.GetComponent<Light>();
                light.type = LightType.Spot;
                light.spotAngle = itemTile.SpotAngle;
                light.color = itemTile.LightColor;
                light.intensity = itemTile.LightIntensity;
                light.range = itemTile.LightRange;
            }

            if (itemTile.InvokeRule == false)
            {
                m_WallsTilemap.SetTile(coordinate, itemTile);
                return true;
            }

            TileBehaviourRule wallSideRule = Singleton_TileLibrary.Instance.ReturnWallSideRuleByTag(itemTile.Tag);
            m_WallsTilemap.SetTile(coordinate, itemTile);

            if (tile == null)
            {
                tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            }
            else
            {
                tile = (TileBlockBase)wallSideRule.TileGroups[0].Tiles[0];
            }
            
            
            
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
                    return true;
                }
                
            }

            return false;
        }

        private Collider2D CheckTriggerCollider(Vector3 pos)
        {
            RaycastHit2D[] hitdata;
            hitdata = Physics2D.BoxCastAll(pos, new Vector2(0.2f, 0.2f), 0, new Vector3 (0,0, -1.34f));
            foreach (RaycastHit2D rh in hitdata)
            {
                if (rh.collider.gameObject.name != "LightCollider")
                {
                    Debug.Log(rh.collider.gameObject.name);
                    continue;
                }
                Collider2D cl = rh.collider;
                if (cl != null)
                {
                    return cl;
                }

            }

            return null;
        }

        private void ParticleSpawner(Vector3 coord, TileBlockBase tile)
        {
            Color c = Color.white;
            c = UsefulBits.GetAverageSpriteColor(tile.sprite);

            GameObject p = Instantiate(Singleton_PrefabLibrary.Instance.CubeParticles);
            p.GetComponent<BlockParticleManipulator>().ChangeParticleColorsAndSprite(c, tile.sprite);
            p.transform.position = coord + new Vector3(0.25f, 0.25f);
        }
    }
}
