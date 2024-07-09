using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Random = UnityEngine.Random;

namespace DecayingEarth
{
    public class Singleton_CropManager : MonoSingleton<Singleton_CropManager>
    {
        [SerializeField] private Tilemap wallTilemap;

        [SerializeField] private int m_CheckTimer;
        /// <summary>
        /// Раз в какое количество секунд будет проводится глобальная проверка;
        /// </summary>
        public int CheckTimer => m_CheckTimer;

        private float m_Timer = 0;

        /// <summary>
        /// Хранит информацию о выращиваемом тайле, используется Crop Manager-ом
        /// </summary>
        [Serializable]
        public struct CropInfo
        {
            public TileBlockGrowablle Tile;
            public Vector3Int Coordinate;

            public CropInfo(TileBlockGrowablle tile, Vector3Int coord)
            {
                Tile = tile;
                Coordinate = coord;
            }
        }

        
        private List<CropInfo> m_CropList = new();
        /// <summary>
        /// Лист всех объектов, что растут на карте.
        /// </summary>
        public List<CropInfo> CropList => m_CropList;






        protected override void Awake()
        {
            m_Timer = m_CheckTimer;
            base.Awake();

        }

        private void Start()
        {
            foreach(Vector3Int pos in wallTilemap.cellBounds.allPositionsWithin)
            {
                if (wallTilemap.GetTile(pos) is TileBlockGrowablle) AddCropToList(pos);
            }
        }

        private void Update()
        {
            if (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
                return;
            }

            m_Timer = m_CheckTimer;

            for (int i = 0; i < m_CropList.Count; i++)
            {
                int check = Random.Range(0, m_CropList[i].Tile.GrowthChance);

                if (check != 0) continue;

                if (m_CropList[i].Tile.CanSpread)
                {
                    TrySpreadingFrom(m_CropList[i].Coordinate, m_CropList[i].Tile.SpreadableBlock);
                }
                GenerateRandomFoliage(m_CropList[i].Coordinate);

                if (m_CropList[i].Tile.NextBlock != null)
                {
                    Singleton_GridLibrary.Instance.WallsTilemap.SetTile(m_CropList[i].Coordinate, m_CropList[i].Tile.NextBlock);
                    if (m_CropList[i].Tile.NextBlock is TileBlockGrowablle && (m_CropList[i].Tile.NextBlock as TileBlockGrowablle).NextBlock != null)
                    {
                        
                        m_CropList[i] = new CropInfo(m_CropList[i].Tile.NextBlock as TileBlockGrowablle, m_CropList[i].Coordinate);
                        continue;
                    }
                    else
                    {
                        m_CropList.RemoveAt(i);
                        i--;
                        continue;
                    }

                }
                else
                {
                    m_CropList.RemoveAt(i);
                    i--;
                    continue;
                }



            }

        }








        /// <summary>
        /// Добавляет растение и его координаты в лист
        /// </summary>
        /// <param name="coord"> Координаты растения</param>
        public void AddCropToList(Vector3Int coord)
        {
            TileBlockGrowablle tile = (TileBlockGrowablle)Singleton_GridLibrary.Instance.WallsTilemap.GetTile(coord);
            int c = CheckForTheCropOnList(coord);
            if (c < 0)
            {
                m_CropList.Add(new CropInfo(tile, coord));
            }
        }

        /// <summary>
        /// Добавляет растение и его координаты в лист
        /// </summary>
        /// <param name="tile">Тайл растения</param>
        /// <param name="coord"> Координаты растения</param>
        public void AddCropToList(TileBlockGrowablle tile, Vector3Int coord)
        {
            int c = CheckForTheCropOnList(coord);
            if (c < 0)
            {
                m_CropList.Add(new CropInfo(tile, coord));
            }
        }

        /// <summary>
        /// Убирает растение из листа
        /// </summary>
        /// <param name="cropPos">Позиция убираемого растения</param>
        public void RemoveCropFromList(Vector3Int cropPos)
        {
            int c = CheckForTheCropOnList(cropPos);
            if (c >= 0)
            {
                m_CropList.RemoveAt(c);
            }
        }

        /// <summary>
        /// Проверяет, есть ли растение в выбранной позиции
        /// </summary>
        /// <param name="cropPos">Позиция</param>
        /// <returns>Возвращает значение позиции, или -1 если ее не нашли</returns>
        public int CheckForTheCropOnList(Vector3Int cropPos)
        {
            for (int i = 0; i < m_CropList.Count; i++)
            {
                if (m_CropList[i].Coordinate == cropPos) return i;
            }

            return -1;
        }

        /// <summary>
        /// Генерирует траву в выбранном месте
        /// </summary>
        /// <param name="pos">Координата</param>
        public void GenerateRandomFoliage(Vector3Int pos)
        {
            if (Singleton_GridLibrary.Instance.FloorDetsTilemap.GetTile(pos) != null) return;

            TileGroup tileGroup = Singleton_TileLibrary.Instance.ReturnTileGroupByTag("CaveGrass");
            TileBlockBase tile = null;
            if (tileGroup != null)
            {
                Debug.Log("Tile Set");
                tile = (TileBlockBase)tileGroup.Tiles[Random.Range(25, 33)];
            }

            if(tile != null)
            {
                Debug.Log("Tile placed");
                Singleton_GridLibrary.Instance.FloorDetsTilemap.SetTile(pos, tile);
            }
        }

        public void TrySpreadingFrom(Vector3Int pos, TileBlockBase tile)
        {
            for (int chance = Random.Range(1, 4); chance > 0; chance--)
            {
                int rndx = Random.Range(-1, 2);
                int rndy = Random.Range(-1, 2);

                if (rndx == 0 && rndy == 0)
                {
                    chance++;
                    continue;
                }

                Vector3Int posnew = new(pos.x + rndx, pos.y + rndy);
                if (Singleton_GridLibrary.Instance.WallsTilemap.GetTile(posnew) == null)
                {
                    Singleton_GridLibrary.Instance.WallsTilemap.SetTile(posnew, tile);
                    //if (tile is TileBlockGrowablle) AddCropToList((tile as TileBlockGrowablle), posnew);

                }

            }
        }




    }
}
