using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using Utility;
using Random = UnityEngine.Random;

namespace DecayingEarth
{
    public class Singleton_BlockEditor : MonoSingleton<Singleton_BlockEditor>
    {
        [SerializeField] private float m_BlockShakeTime = 1f;
        [SerializeField] private float m_BlockShakeIntensity = 1f;
        [SerializeField] private float m_BlockShakesPerSecond = 8f;

        private Tilemap m_WallsTilemap;
        private Tilemap m_OresTilemap;
        private Tilemap m_FloorTilemap;

        private void Start()
        {
            m_WallsTilemap = Singleton_GridLibrary.Instance.WallsTilemap;
            m_OresTilemap = Singleton_GridLibrary.Instance.OresTilemap;
            m_FloorTilemap = Singleton_GridLibrary.Instance.FloorTilemap;
        }

        /// <summary>
        /// ���������� ����
        /// </summary>
        /// <param name="damage">����, ��������� �����</param>
        /// <param name="maxDistance">������������ ��������� �� ������ ��� ��������� �����</param>
        /// <param name="radius">������ �������������� ����</param>
        /// <returns>True ���� ��������� �������</returns>
        public bool DamageWallBlock(int damage, float maxDistance = 3, int radius = 1)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return false;

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float dist = Vector2.Distance(Camera.main.transform.position, pos);
            if (dist > maxDistance) return false;

            Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
            TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            if (tile == null) return false;

            if (tile.IsALightSource)
            {
                Collider2D col = CheckTriggerCollider(m_WallsTilemap.CellToWorld(coordinate) + new Vector3(0.25f, 0.25f), "LightCollider", new Vector2(0.2f, 0.2f), new Vector3(0, 0, -1.34f));

                if (col != null && tile.RemainingDurability - damage <= 0) Destroy(col.transform.root.gameObject);
            }

            PlaceOrRemoveInvisibleTiles(coordinate, tile, m_WallsTilemap, false);

            if (tile.InvokeRule == false)
            {
                int j = 1;
                j = tile.DealDamage(damage, m_WallsTilemap.CellToWorld(coordinate), coordinate);

                if (j == 0)
                {
                    StopAllCoroutines();
                    if (tile != null) UsefulBits.FixTilePosition(coordinate, m_WallsTilemap, tile);
                    m_WallsTilemap.SetTile(coordinate, null);
                    ParticleSpawner(m_WallsTilemap.CellToWorld(coordinate), tile);
                }
                else
                {
                    int rdseed = Random.Range(0, 500);
                    if (tile != null) StartCoroutine(UsefulBits.TileShaker(coordinate, m_WallsTilemap, tile, m_BlockShakeTime, m_BlockShakeIntensity, m_BlockShakesPerSecond, rdseed));
                }
                return true;
            }
            else
            {
                TileBlockBase ore = m_OresTilemap.GetTile<TileBlockBase>(coordinate);

                if (Singleton_SessionData.Instance.LastTileCoordinate != (Vector2Int)coordinate)
                {
                    tile.Recover();
                    if (ore != null) ore.Recover();
                }
                Singleton_SessionData.Instance.UpdateLastTileCoordinate((Vector2Int)coordinate);

                int i = 1;
                if (ore == null || ore.MaxDurability < tile.MaxDurability) i = tile.DealDamage(damage, m_WallsTilemap.CellToWorld(coordinate), coordinate);
                else i = ore.DealDamage(damage, m_WallsTilemap.CellToWorld(coordinate), coordinate);

                if (i == 0)
                {
                    StopAllCoroutines();

                    if (tile != null) UsefulBits.FixTilePosition(coordinate, m_WallsTilemap, tile);
                    if (ore != null) UsefulBits.FixTilePosition(coordinate, m_OresTilemap, ore);

                    if (ore != null && ore.MaxDurability >= tile.MaxDurability)
                    {
                        m_OresTilemap.SetTile(coordinate, null);
                        tile.DealDamage(1000000, m_WallsTilemap.CellToWorld(coordinate), coordinate);
                    }
                    if (ore != null && ore.MaxDurability < tile.MaxDurability)
                    {
                        ore.DealDamage(1000000, m_OresTilemap.CellToWorld(coordinate), coordinate);
                        m_OresTilemap.SetTile(coordinate, null);
                    }

                    ParticleSpawner(m_WallsTilemap.CellToWorld(coordinate), tile);
                    m_WallsTilemap.SetTile(coordinate, null);

                    WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, tile, radius, false);
                }
                else
                {
                    int rdseed = Random.Range(0, 500);
                    if (tile != null) StartCoroutine(UsefulBits.TileShaker(coordinate, m_WallsTilemap, tile, m_BlockShakeTime, m_BlockShakeIntensity, m_BlockShakesPerSecond, rdseed));
                    if (ore != null) StartCoroutine(UsefulBits.TileShaker(coordinate, m_OresTilemap, ore, m_BlockShakeTime, m_BlockShakeIntensity, m_BlockShakesPerSecond, rdseed));
                }
                return true;
            }
        }

        /// <summary>
        /// ������ ����, �������� � ��������
        /// </summary>
        /// <param name="itemTile">��������������� ����</param>
        /// <param name="maxDistance">������������ ��������� �� ������ ��� ��������� �����</param>
        /// <param name="radius">������ �������������� ����</param>
        ///<returns>True ���� ��������� �������</returns>
        public bool PlaceWallBlock(TileBlockBase itemTile, float maxDistance = 3, int radius = 1)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return false;

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!itemTile.IgnoreRigidbody && CheckRigidbody(pos) == true) return false;
            float dist = Vector2.Distance(Camera.main.transform.position, pos);
            if (dist > maxDistance) { Debug.Log("Position:" + dist); return false; }
            Vector3Int coordinate = m_WallsTilemap.WorldToCell(pos);
            TileBlockBase checkTile = m_WallsTilemap.GetTile<TileBlockBase>(coordinate);
            if (checkTile != null) return false;

            if (!CheckAvailablePlaceBySize(itemTile, m_WallsTilemap, coordinate)) return false;

            if (itemTile is TileBlockStorageChest) Singleton_GlobalChestController.Instance.AddInventory(coordinate);
            
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

            PlaceOrRemoveInvisibleTiles(coordinate, itemTile, m_WallsTilemap, true);

            if (itemTile.InvokeRule == false)
            {
                m_WallsTilemap.SetTile(coordinate, itemTile);
                return true;
            }
            else Singleton_GridLibrary.Instance.FloorDetsTilemap.SetTile(coordinate, null);


            //Debug.Log(coordinate);
            WorldShaper.EditWallsAroundPoint(coordinate.x, coordinate.y, m_WallsTilemap, itemTile, radius, true);

            return true;
        }

        /// <summary>
        /// ������ ��� ������� ��������� ����� ��� ������� ��������
        /// </summary>
        /// <param name="startPos">��������� �������</param>
        /// <param name="tile">��������� �����</param>
        /// <param name="wall">������� �������</param>
        /// <param name="mode">true - ������, false - �������</param>
        private void PlaceOrRemoveInvisibleTiles(Vector3Int startPos, TileBlockBase tile, Tilemap wall ,bool mode)
        {
            Vector2Int dim = tile.Size;
            if (dim == Vector2Int.one) return;

            if (dim.x <= 0 || dim.y <= 0)
            {
                Debug.LogError("Size of the tile " + tile.name + " is set wrongly! Aborting placement.");
                return;
            }

            for (int i = 0; i < dim.x; i++)
            {
                for (int j = 0; j < dim.y; j++)
                {
                    if (mode && (i != 0 || j != 0) ) wall.SetTile(new Vector3Int(startPos.x + i, startPos.y + j), Singleton_PrefabLibrary.Instance.FillerTile) ;
                    if (!mode && (i != 0 || j != 0) ) wall.SetTile(new Vector3Int(startPos.x + i, startPos.y + j), null) ;
                }
            }
        }


        /// <summary>
        /// ��������� ����������� ��������� ����� � ������������ ������� � ��������� ��������
        /// </summary>
        /// <param name="tile">��������������� ����</param>
        /// <param name="wall">������� �������</param>
        /// <param name="startPos">��������� ������� ��������</param>
        /// <returns></returns>
        private bool CheckAvailablePlaceBySize(TileBlockBase tile, Tilemap wall, Vector3Int startPos)
        {
             Vector2Int dim = tile.Size;
            if (dim == Vector2Int.one) return true;

            if (dim.x <= 0 || dim.y <= 0)
            {
                Debug.LogError("Size of the tile " + tile.name + " is set wrongly! Aborting placement.");
                return false;
            }

            for (int i = 0; i < dim.x; i++)
            {
                for (int j = 0; j < dim.y; j++)
                {
                    if (wall.GetTile(new Vector3Int(i + startPos.x, j + startPos.y, 0)) != null) return false;
                }
            }

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

        private Collider2D CheckTriggerCollider(Vector3 pos, string tag, Vector2 size, Vector3 castPos)
        {
            RaycastHit2D[] hitdata;
            hitdata = Physics2D.BoxCastAll(pos, size, 0, castPos);
            foreach (RaycastHit2D rh in hitdata)
            {
                if (rh.collider.gameObject.name != tag)
                {
                    //Debug.Log(rh.collider.gameObject.name);
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

        private void ParticleSpawner(Vector3 coord, TileBlockBase tile, int amountOfParticles = 200)
        {
            Color c = UsefulBits.GetAverageSpriteColor(tile.sprite);

            GameObject p = Instantiate(Singleton_PrefabLibrary.Instance.CubeParticles);
            p.GetComponent<BlockParticleManipulator>().ChangeParticles(c, tile.sprite, amountOfParticles);
            p.transform.position = coord + new Vector3(0.25f, 0.25f);
        }
    }
}
