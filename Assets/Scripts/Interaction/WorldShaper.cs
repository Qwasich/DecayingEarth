using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace DecayingEarth
{
    public static class WorldShaper
    {
        /// <summary>
        /// Редактирует стены и подбирает нужные формы в зависимости от правил.
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="cavegen">Если указан - будет брать правила тайлов оттуда а не из библиотеки. ОБЯЗАТЕЛЬНО ДЛЯ ГЕНЕРАЦИИ В РЕДАКТОРЕ.</param>
        public static void PlaceEditedWallsAltRule(int x, int y, Tilemap wall, Singleton_CaveGenerator cavegen = null)
        {
            TileBlockBase tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y, 0));
            if (tile == null) return;
            if (!tile.InvokeRule) return;
            if (tile.BlockTag == "")
            {
                Debug.LogError("Tile " + tile.name +" has no tag set, block generation failure"); 
                return;
            }

            TileGroup tileGroup = null;
            if (cavegen != null)
            {
                for (int i = 0; i < cavegen.TileRule.Length; i++)
                {
                    if (tile.BlockTag == cavegen.TileRule[i].BlockTag)
                    {
                        tileGroup = cavegen.TileRule[i];
                        break;
                    }

                }

                if (tileGroup == null)
                {
                    Debug.LogError("Proper Tile Rule isn't set for the tile tag " + tile.BlockTag + " on the latest used world generation settings."); return;
                }
            }
            else
            {
                tileGroup = Singleton_TileLibrary.Instance.ReturnTileGroupByTag(tile.BlockTag);
            }

            int lt = 0;
            int t = 0;
            int rt = 0;
            int l = 0;
            int r = 0;
            int ld = 0;
            int d = 0;
            int rd = 0;

            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y + 1));
            if (tile != null && tile.InvokeRule) lt = 1;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y + 1));
            if (tile != null && tile.InvokeRule) t = 1;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y + 1));
            if (tile != null && tile.InvokeRule) rt = 1;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y));
            if (tile != null && tile.InvokeRule) l = 1;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y));
            if (tile != null && tile.InvokeRule) r = 1;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y - 1));
            if (tile != null && tile.InvokeRule) ld = 1;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y - 1));
            if (tile != null && tile.InvokeRule) d = 1;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y - 1));
            if (tile != null && tile.InvokeRule) rd = 1;


            //обычные стены
            if (t == 0 && l == 0 && r == 1 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[0]); return; }
            if (t == 0 && l == 1 && r == 1 && rd == 1 && d == 1 && ld == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[1]); return; }
            if (t == 0 && l == 1 && r == 0 && ld == 1 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[2]); return; }

            if (t == 1 && rt == 1 && l == 0 && r == 1 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[3]); return; }
            if (lt == 1 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[4]); return; }
            if (lt == 1 && t == 1 && l == 1 && r == 0 && ld == 1 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[5]); return; }

            if (t == 1 && rt == 1 && l == 0 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[6]); return; }
            if (lt == 1 && t == 1 && rt == 1 && l == 1 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[7]); return; }
            if (lt == 1 && t == 1 && l == 1 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[8]); return; }

            //Вертикальные стены
            if (t == 0 && l == 0 && r == 0 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[9]); return; }
            if (t == 1 && l == 0 && r == 0 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[10]); return; }
            if (t == 1 && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[11]); return; }

            //Замкнутые стены
            if (t == 0 && l == 0 && r == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[12]); return; }
            if (t == 0 && l == 1 && r == 1 && rd == 0 && d == 1 && ld == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[13]); return; }
            if (t == 0 && l == 1 && r == 0 && ld == 0 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[14]); return; }

            if (t == 1 && rt == 0 && l == 0 && r == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[15]); return; }
            if (lt == 0 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[16]); return; }
            if (lt == 0 && t == 1 && l == 1 && r == 0 && ld == 0 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[17]); return; }

            if (t == 1 && rt == 0 && l == 0 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[18]); return; }
            if (lt == 0 && t == 1 && rt == 0 && l == 1 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[19]); return; }
            if (lt == 0 && t == 1 && l == 1 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[20]); return; }

            //Горизонтальные стены
            if (t == 0 && l == 0 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[21]); return; }
            if (t == 0 && l == 1 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[22]); return; }
            if (t == 0 && l == 1 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[23]); return; }

            //Колонна
            if (t == 0 && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[24]); return; }

            //Внешние особые углы
            if (t == 0 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[25]); return; }
            if (t == 0 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[26]); return; }
            if (t == 1 && rt == 1 && l == 0 && r == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[27]); return; }
            if (lt == 1 && t == 1 && l == 1 && r == 0 && ld == 0 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[28]); return; }
            if (t == 1 && rt == 0 && l == 0 && r == 1 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[29]); return; }
            if (lt == 0 && t == 1 && l == 1 && r == 0 && ld == 1 && d == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[30]); return; }
            if (lt == 1 && t == 1 && rt == 0 && l == 1 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[31]); return; }
            if (lt == 0 && t == 1 && rt == 1 && l == 1 && r == 1 && d == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[32]); return; }

            //Внутренние особые углы
            if (lt == 1 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[33]); return; }
            if (lt == 1 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[34]); return; }
            if (lt == 1 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[35]); return; }
            if (lt == 0 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[36]); return; }

            if (lt == 0 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[37]); return; }
            if (lt == 1 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[38]); return; }
            if (lt == 0 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[39]); return; }
            if (lt == 1 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[40]); return; }

            if (lt == 1 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[41]); return; }
            if (lt == 0 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[42]); return; }

            if (lt == 0 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 1) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[43]); return; }
            if (lt == 0 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 1 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[44]); return; }
            if (lt == 0 && t == 1 && rt == 1 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[45]); return; }
            if (lt == 1 && t == 1 && rt == 0 && l == 1 && r == 1 && ld == 0 && d == 1 && rd == 0) { wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[46]); return; }
        }


        public static BlockType GetTileType(Tilemap target, int x, int y)
        {
            TileBlockBase tile = target.GetTile<TileBlockBase>(new Vector3Int(x, y));
            if (tile == null) return BlockType.FLOOR;
            return tile.BlockType;
        }

        public static string GetTileTag(Tilemap target, int x, int y)
        {
            TileBlockBase tile = target.GetTile<TileBlockBase>(new Vector3Int(x, y));
            return tile.BlockTag;
        }

        /// <summary>
        /// Редактирует стены и подбирает нужные формы в зависимости от правил.
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="tile">Настройки разрушенного или поставленного тайла</param>
        /// <param name="radius">Радиус обновления блоков вокруг центрального тайла. Всегда равняется минимум 1.</param>
        /// <param name="mode">false - Режим разрушения, true - режим установки</param>
        public static void EditWallsAroundPoint(int x, int y, Tilemap wall, TileBlockBase tile, int radius, bool mode = false)
        {
            TileGroup tileGroup;

            tileGroup = Singleton_TileLibrary.Instance.ReturnTileGroupByTag(tile.BlockTag);

            if (mode == false)
            {
                wall.SetTile(new Vector3Int(x, y), null);
            }

            if (mode == true) wall.SetTile(new Vector3Int(x, y), tileGroup.Tiles[4]);

            Singleton_SessionData.Instance.UpdateLastTileCoordinate(new Vector2Int(x,y));

            if (radius <= 0) radius = 1;

            for (int i = x - radius; i <= radius + x; i++)
            {
                for (int j = y - radius; j <= radius + y; j++)
                {
                    PlaceEditedWallsAltRule(i, j, wall);
                }
            }
        }
    }
}
