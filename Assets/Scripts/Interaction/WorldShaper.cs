using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace ScorgedEarth
{
    public static class WorldShaper
    {
        /// <summary>
        /// Редактирует стены и подбирает нужные формы в зависимости от правил.
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="wallSideRule">Правило передних стен</param>
        /// <param name="wallTopRule">Правило верхних стен</param>
        [Obsolete("Use Alt Rule instead. This metod wasn't updated, but still a great coding reference.")]
        public static void PlaceEditedWalls(int x, int y, Tilemap wall, TileBehaviourRule wallSideRule, TileBehaviourRule wallTopRule)
        {
            if (wall.GetTile(new Vector3Int(x, y, 0)) == null) return;
            bool c = true;

            bool lt = false;
            bool t = false;
            bool rt = false;
            bool l = false;
            bool r = false;
            bool ld = false;
            bool d = false;
            bool rd = false;

            if (wall.GetTile(new Vector3Int(x - 1, y + 1, 0)) != null) lt = true;
            if (wall.GetTile(new Vector3Int(x, y + 1, 0)) != null) t = true;
            if (wall.GetTile(new Vector3Int(x + 1, y + 1, 0)) != null) rt = true;
            if (wall.GetTile(new Vector3Int(x - 1, y, 0)) != null) l = true;
            if (wall.GetTile(new Vector3Int(x + 1, y, 0)) != null) r = true;
            if (wall.GetTile(new Vector3Int(x - 1, y - 1, 0)) != null) ld = true;
            if (wall.GetTile(new Vector3Int(x, y - 1, 0)) != null) d = true;
            if (wall.GetTile(new Vector3Int(x + 1, y - 1, 0)) != null) rd = true;

            //доп проверка
            if (lt != c && t != c && rt != c && l != c && r != c && ld != c && d != c && rd != c) wall.SetTile(new Vector3Int(x, y, 0), null);
            if (t != c && d != c) wall.SetTile(new Vector3Int(x, y, 0), null);
            
            //Нижние стены
            if (t == c && l == c && r == c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); return; }
            if (t == c && l == c && r != c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); return; }
            if (t == c && l != c && r == c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); return; }
            if (t == c && l != c && r != c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); return; }

            //правые стены
            if (lt == c && t == c && l == c && r != c && ld == c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }
            if (lt == c && t == c && l == c && r == c && ld == c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }

            //левые стены
            if (t == c && rt == c && l != c && r == c && d == c && rd == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[2].Tiles[Random.Range(0, wallTopRule.TileGroups[2].Tiles.Length)]); return; }
            if (t == c && rt == c && l == c && r == c && ld != c && d == c && rd == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[2].Tiles[Random.Range(0, wallTopRule.TileGroups[2].Tiles.Length)]); return; }

            //обе стены
            if (t == c && l != c && r != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }
            if (t == c && l == c && r != c && ld != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }
            if (t == c && l != c && r == c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }
            if (t == c && l == c && r == c && ld != c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }

            //обе стены сверху
            if (t != c && l != c && r != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[4].Tiles[Random.Range(0, wallTopRule.TileGroups[4].Tiles.Length)]); return; }

            //правый угол
            if (t != c && l == c && r != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[5].Tiles[Random.Range(0, wallTopRule.TileGroups[5].Tiles.Length)]); return; }
            if (t != c && l == c && r == c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[5].Tiles[Random.Range(0, wallTopRule.TileGroups[5].Tiles.Length)]); return; }

            //левый угол
            if (t != c && l != c && r == c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[6].Tiles[Random.Range(0, wallTopRule.TileGroups[6].Tiles.Length)]); return; }
            if (t != c && l == c && r == c && ld != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[6].Tiles[Random.Range(0, wallTopRule.TileGroups[6].Tiles.Length)]); return; }

            //только верх
            if (t != c && l == c && r == c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[7].Tiles[Random.Range(0, wallTopRule.TileGroups[7].Tiles.Length)]); return; }

            //обратные углы
            if (lt != c && t == c && rt == c && l == c && r == c && rd == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[0]); return; }
            if (lt == c && t == c && rt != c && l == c && r == c && ld == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[1]); return; }
            if (lt != c && t == c && rt != c && l == c && r == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[2]); return; }
            if (lt != c && t == c && l == c && r != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[3]); return; }
            if (lt != c && t == c && l == c && r == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[3]); return; }
            if (t == c && rt != c && r != c && l == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[4]); return; }
            if (t == c && rt != c && r == c && l == c && ld != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[4]); return; }

        }

        /// <summary>
        /// Редактирует стены и подбирает нужные формы в зависимости от правил.
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="mode">0 - Полный, 1 - Только передние, 2 - Только верхние</param>
        /// <param name="cavegen">Если указан - будет брать правила тайлов оттуда а не из библиотеки. ОБЯЗАТЕЛЬНО ДЛЯ ГЕНЕРАЦИИ В РЕДАКТОРЕ.</param>
        public static void PlaceEditedWallsAltRule(int x, int y, Tilemap wall, int mode = 0, Cave_Generator cavegen = null)
        {
            if (mode > 2 || mode < 0) { Debug.LogError("Alt Rule Generation mode set wrongly! Must use value between 0 and 2, inclusive."); return; }
            TileBlockBase tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y, 0));
            if (tile == null) return;
            if (tile.Tag == "")
            {
                Debug.LogError("Tile " + tile.name +" has no tag set, block generation failure"); 
                return;
            }

            TileBehaviourRule wallSideRule;
            TileBehaviourRule wallTopRule;
            if (cavegen != null)
            {
                wallSideRule = cavegen.WallFrontRule;
                wallTopRule = cavegen.WallTopRule;
            }
            else
            {
                wallSideRule = Singleton_TileLibrary.Instance.ReturnWallSideRuleByTag(tile.Tag);
                wallTopRule = Singleton_TileLibrary.Instance.ReturnWallTopRuleByTag(tile.Tag);
            }
            
            int c = 2;

            int lt = 0;
            int t = 0;
            int rt = 0;
            int l = 0;
            int r = 0;
            int ld = 0;
            int d = 0;
            int rd = 0;

            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y + 1));
            if (tile != null) lt = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y + 1));
            if (tile != null) t = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y + 1));
            if (tile != null) rt = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y));
            if (tile != null) l = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y));
            if (tile != null) r = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y - 1));
            if (tile != null) ld = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y - 1));
            if (tile != null) d = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y - 1));
            if (tile != null) rd = (int)tile.BlockType;

            //доп проверка
            //if (lt == c && t == c && rt == c && l == c && r != c && ld != c && d != c && rd != c) wall.SetTile(new Vector3Int(x, y, 0), null);
            if (t == 0 && d == 0) wall.SetTile(new Vector3Int(x, y, 0), null);

            if (mode == 0 || mode == 1)
            {
                //Нижние стены
                if (t == c && l != 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                if (t == c && l != 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                if (t == c && l == 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                if (t == c && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }
            }

            if (mode == 1) return;

            if (GetTileType(wall, x, y) == BlockType.SIDE) return;

            //правые стены
            if (lt == c && t == c && l == c && r != c && ld != 0 && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }
            if (lt == c && t == c && l == c && r == c && ld != 0 && d != 0 && rd == 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }

            //левые стены
            if (t == c && rt == c && l != c && r == c && d != 0 && rd != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[2].Tiles[Random.Range(0, wallTopRule.TileGroups[2].Tiles.Length)]); return; }

            //обе стены
            if (t == c && l != c && r != c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }

            //обе стены сверху
            if (t != c && l != c && r != c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[4].Tiles[Random.Range(0, wallTopRule.TileGroups[4].Tiles.Length)]); return; }

            //правый угол
            if (t != c && l == c && r != c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[5].Tiles[Random.Range(0, wallTopRule.TileGroups[5].Tiles.Length)]); return; }

            //левый угол
            if (t != c && l != c && r == c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[6].Tiles[Random.Range(0, wallTopRule.TileGroups[6].Tiles.Length)]); return; }

            //только верх
            if (t != c && l == c && r == c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[7].Tiles[Random.Range(0, wallTopRule.TileGroups[7].Tiles.Length)]); return; }

            //обратные углы
            if (lt != c && t == c && rt == c && l == c && r == c && d != 0 && rd != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[8].Tiles[0]); return; }
            if (lt == c && t == c && rt != c && l == c && r == c && ld != 0 && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[8].Tiles[1]); return; }
            if (lt != c && t == c && rt != c && l == c && r == c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[8].Tiles[2]); return; }
            if (lt != c && t == c && l == c && r != c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[8].Tiles[3]); return; }
            if (lt != c && t == c && l == c && r == c && d != 0 && rd == 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[8].Tiles[3]); return; }
            if (t == c && rt != c && l != c && r == c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[8].Tiles[4]); return; }

            if (mode != 2) return;
            if (lt == c && t == c && rt == c && l == c & r == c && ld != 0 && d != 0 && rd != 0) {wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[0].Tiles[0]); return;}

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
            return tile.Tag;
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
            TileBehaviourRule wallSideRule = Singleton_TileLibrary.Instance.ReturnWallSideRuleByTag(tile.Tag);
            TileBehaviourRule wallTopRule = Singleton_TileLibrary.Instance.ReturnWallTopRuleByTag(tile.Tag);

            int c = 2;

            //int lt = 0;
            int t = 0;
            //int rt = 0;
            int l = 0;
            int r = 0;
            //int ld = 0;
            int d = 0;
            //int rd = 0;

            //if (wall.GetTile(new Vector3Int(x - 1, y + 1)) != null) lt = (int)GetTileType(wall, x - 1, y + 1);
            if (wall.GetTile(new Vector3Int(x, y + 1)) != null) t = (int)GetTileType(wall, x, y + 1);
            //if (wall.GetTile(new Vector3Int(x + 1, y + 1)) != null) rt = (int)GetTileType(wall, x + 1, y + 1);
            if (wall.GetTile(new Vector3Int(x - 1, y)) != null) l = (int)GetTileType(wall, x - 1, y);
            if (wall.GetTile(new Vector3Int(x + 1, y)) != null) r = (int)GetTileType(wall, x + 1, y);
            //if (wall.GetTile(new Vector3Int(x - 1, y - 1)) != null) ld = (int)GetTileType(wall, x - 1, y - 1);
            if (wall.GetTile(new Vector3Int(x, y - 1)) != null) d = (int)GetTileType(wall, x, y - 1);
            //if (wall.GetTile(new Vector3Int(x + 1, y - 1)) != null) rd = (int)GetTileType(wall, x + 1, y - 1);

            if (mode == false)
            {

                if (tile.BlockType == BlockType.TOP)
                {
                    TileBlockBase tl = (TileBlockBase)wall.GetTile(new Vector3Int(x, y - 1));
                    if (tl != null && tl.BlockType == BlockType.SIDE)
                    {
                        wall.SetTile(new Vector3Int(x, y - 1), null);
                        if (wall.GetTile(new Vector3Int(x, y - 1)) == null) d = 0;
                    }
                    tl = (TileBlockBase)wall.GetTile(new Vector3Int(x, y + 1));
                    TileBehaviourRule oldSideRule = null;
                    if (tl != null && tile.Tag !=tl.Tag && tl.BlockType == BlockType.TOP)
                    {
                        oldSideRule = wallSideRule;
                        wallSideRule = Singleton_TileLibrary.Instance.ReturnWallSideRuleByTag(tl.Tag);
                        if (wallSideRule != null) { }
                        else wallSideRule = oldSideRule;
                    }
                    //Нижние стены
                    if (t == c && l != 0 && r != 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                    else if (t == c && l != 0 && r == 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                    else if (t == c && l == 0 && r != 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                    else if (t == c && l == 0 && r == 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }
                    if (oldSideRule != null) wallSideRule = oldSideRule;

                }

                if (tile.BlockType == BlockType.SIDE)
                {
                    TileBlockBase tl = (TileBlockBase)wall.GetTile(new Vector3Int(x, y + 2));
                    if (tl == null || tl.BlockType == BlockType.SIDE)
                    {
                        wall.SetTile(new Vector3Int(x, y + 1), null);
                        if (wall.GetTile(new Vector3Int(x, y - 1)) != null) d = (int)GetTileType(wall, x, y - 1);
                        t = 0;
                    }
                    TileBehaviourRule oldSideRule = null;
                    if (tl != null && tile.Tag != tl.Tag && tl.BlockType == BlockType.SIDE)
                    {
                        oldSideRule = wallSideRule;
                        wallSideRule = Singleton_TileLibrary.Instance.ReturnWallSideRuleByTag(tl.Tag);
                        if (wallSideRule != null) { }
                        else wallSideRule = oldSideRule;
                    }
                    else if (t == c && l != 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                    else if (t == c && l != 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                    else if (t == c && l == 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                    else if (t == c && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }
                    if (oldSideRule != null) wallSideRule = oldSideRule;
                }
               
            }

            if (mode == true)
            {
                int m = 1;
                TileBlockBase tl = (TileBlockBase)wall.GetTile(new Vector3Int(x, y - 1));
                if (tile.BlockType == BlockType.TOP)
                {
                    TileBlockBase ml = (TileBlockBase)wall.GetTile(new Vector3Int(x, y + 1));
                    if (tl == null)
                    {
                        wall.SetTile(new Vector3Int(x, y + 1), wallTopRule.TileGroups[0].Tiles[0]);
                        m = 0;
                    }
                    if (tl != null && ml!= null && ml.BlockType == BlockType.SIDE && Singleton_SessionData.Instance.IsTop == false)
                    {
                        wall.SetTile(new Vector3Int(x, y + 1), wallTopRule.TileGroups[0].Tiles[0]);
                        d = 0;
                        t = c;
                        m = 0;
                    }
                    if (tl != null && ml != null && ml.BlockType == BlockType.SIDE && Singleton_SessionData.Instance.IsTop == true)
                    {
                        m = 0;
                    }
                }

                if (tile.BlockType == BlockType.SIDE)
                {
                    if (tl == null)
                    {
                        wall.SetTile(new Vector3Int(x, y - 1), wallTopRule.TileGroups[0].Tiles[0]);
                    }
                }

                if (tl == null)
                {
                    if (wall.GetTile(new Vector3Int(x, y)) != null) t = (int)GetTileType(wall, x, y);
                    if (wall.GetTile(new Vector3Int(x - 1, y - 1)) != null) l = (int)GetTileType(wall, x - 1, y - m);
                    if (wall.GetTile(new Vector3Int(x + 1, y - 1)) != null) r = (int)GetTileType(wall, x + 1, y - m);
                    d = 0;
                }
                

                //Нижние стены
                if (t == c && l != 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - m), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                else if (t == c && l != 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - m), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                else if (t == c && l == 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - m), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                else if (t == c && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - m), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }

            }

            Singleton_SessionData.Instance.UpdateLastTileCoordinate(new Vector2Int(x,y));

            if (radius <= 0) radius = 1;

            for (int i = x - radius; i <= radius + x; i++)
            {
                for (int j = y - radius; j <= radius + y; j++)
                {
                    if (i == j && i==x && j == y && !mode) continue;
                    PlaceEditedWallsAltRule(i, j, wall, 1);
                }
            }
            for (int i = x - radius; i <= radius + x; i++)
            {
                for (int j = y - radius; j <= radius + y; j++)
                {
                    if (i == j && i == x && j == y && !mode) continue;
                    PlaceEditedWallsAltRule(i, j, wall, 2);
                }
            }
        }
    }
}
