using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace DecayingEarth
{
    public static class WorldShaper
    {
        /// <summary>
        /// ����������� ����� � ��������� ������ ����� � ����������� �� ������.
        /// </summary>
        /// <param name="x">���������� X</param>
        /// <param name="y">���������� Y</param>
        /// <param name="wall">������� �������</param>
        /// <param name="wallSideRule">������� �������� ����</param>
        /// <param name="wallTopRule">������� ������� ����</param>
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

            //��� ��������
            if (lt != c && t != c && rt != c && l != c && r != c && ld != c && d != c && rd != c) wall.SetTile(new Vector3Int(x, y, 0), null);
            if (t != c && d != c) wall.SetTile(new Vector3Int(x, y, 0), null);
            
            //������ �����
            if (t == c && l == c && r == c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); return; }
            if (t == c && l == c && r != c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); return; }
            if (t == c && l != c && r == c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); return; }
            if (t == c && l != c && r != c && d != c) { wall.SetTile(new Vector3Int(x, y, 0), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); return; }

            //������ �����
            if (lt == c && t == c && l == c && r != c && ld == c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }
            if (lt == c && t == c && l == c && r == c && ld == c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }

            //����� �����
            if (t == c && rt == c && l != c && r == c && d == c && rd == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[2].Tiles[Random.Range(0, wallTopRule.TileGroups[2].Tiles.Length)]); return; }
            if (t == c && rt == c && l == c && r == c && ld != c && d == c && rd == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[2].Tiles[Random.Range(0, wallTopRule.TileGroups[2].Tiles.Length)]); return; }

            //��� �����
            if (t == c && l != c && r != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }
            if (t == c && l == c && r != c && ld != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }
            if (t == c && l != c && r == c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }
            if (t == c && l == c && r == c && ld != c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }

            //��� ����� ������
            if (t != c && l != c && r != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[4].Tiles[Random.Range(0, wallTopRule.TileGroups[4].Tiles.Length)]); return; }

            //������ ����
            if (t != c && l == c && r != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[5].Tiles[Random.Range(0, wallTopRule.TileGroups[5].Tiles.Length)]); return; }
            if (t != c && l == c && r == c && d == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[5].Tiles[Random.Range(0, wallTopRule.TileGroups[5].Tiles.Length)]); return; }

            //����� ����
            if (t != c && l != c && r == c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[6].Tiles[Random.Range(0, wallTopRule.TileGroups[6].Tiles.Length)]); return; }
            if (t != c && l == c && r == c && ld != c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[6].Tiles[Random.Range(0, wallTopRule.TileGroups[6].Tiles.Length)]); return; }

            //������ ����
            if (t != c && l == c && r == c && d == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[7].Tiles[Random.Range(0, wallTopRule.TileGroups[7].Tiles.Length)]); return; }

            //�������� ����
            if (lt != c && t == c && rt == c && l == c && r == c && rd == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[0]); return; }
            if (lt == c && t == c && rt != c && l == c && r == c && ld == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[1]); return; }
            if (lt != c && t == c && rt != c && l == c && r == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[2]); return; }
            if (lt != c && t == c && l == c && r != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[3]); return; }
            if (lt != c && t == c && l == c && r == c && rd != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[3]); return; }
            if (t == c && rt != c && r != c && l == c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[4]); return; }
            if (t == c && rt != c && r == c && l == c && ld != c) { wall.SetTile(new Vector3Int(x, y, 0), wallTopRule.TileGroups[8].Tiles[4]); return; }

        }

        /// <summary>
        /// ����������� ����� � ��������� ������ ����� � ����������� �� ������.
        /// </summary>
        /// <param name="x">���������� X</param>
        /// <param name="y">���������� Y</param>
        /// <param name="wall">������� �������</param>
        /// <param name="mode">0 - ������, 1 - ������ ��������, 2 - ������ �������</param>
        /// <param name="cavegen">���� ������ - ����� ����� ������� ������ ������ � �� �� ����������. ����������� ��� ��������� � ���������.</param>
        public static void PlaceEditedWallsAltRule(int x, int y, Tilemap wall, int mode = 0, Singletone_CaveGenerator cavegen = null)
        {
            if (mode > 2 || mode < 0) { Debug.LogError("Alt Rule Generation mode set wrongly! Must use value between 0 and 2, inclusive."); return; }
            TileBlockBase tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y, 0));
            if (tile == null) return;
            if (!tile.InvokeRule) return;
            if (tile.Tag == "")
            {
                Debug.LogError("Tile " + tile.name +" has no tag set, block generation failure"); 
                return;
            }

            TileBehaviourRule wallSideRule = null;
            TileBehaviourRule wallTopRule = null;
            if (cavegen != null)
            {
                for (int i = 0; i < cavegen.WallFrontRule.Length; i++)
                {
                    if (tile.Tag == cavegen.WallFrontRule[i].Tag)
                    {
                        wallSideRule = cavegen.WallFrontRule[i];
                        break;
                    }
                }

                for (int i = 0; i < cavegen.WallTopRule.Length; i++)
                {
                    if (tile.Tag == cavegen.WallTopRule[i].Tag)
                    {
                        wallTopRule = cavegen.WallTopRule[i];
                        break;
                    }

                }

                if (wallTopRule == null || wallSideRule == null)
                {
                    Debug.LogError("Proper Top Tile Rule isn't set for the tile tag " + tile.Tag + " on the latest used world generation settings."); return;
                }
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
            if (tile != null && tile.InvokeRule) lt = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y + 1));
            if (tile != null && tile.InvokeRule) t = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y + 1));
            if (tile != null && tile.InvokeRule) rt = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y));
            if (tile != null && tile.InvokeRule) l = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y));
            if (tile != null && tile.InvokeRule) r = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y - 1));
            if (tile != null && tile.InvokeRule) ld = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y - 1));
            if (tile != null && tile.InvokeRule) d = (int)tile.BlockType;
            tile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y - 1));
            if (tile != null && tile.InvokeRule) rd = (int)tile.BlockType;

            //��� ��������
            //if (lt == c && t == c && rt == c && l == c && r != c && ld != c && d != c && rd != c) wall.SetTile(new Vector3Int(x, y, 0), null);
            if (t == 0 && d == 0) wall.SetTile(new Vector3Int(x, y, 0), null);

            if (mode == 0 || mode == 1)
            {
                //������ �����
                if (t == c && l != 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                if (t == c && l != 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                if (t == c && l == 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                if (t == c && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }
            }

            if (mode == 1) return;

            if (GetTileType(wall, x, y) == BlockType.SIDE) return;

            //������ �����
            if (lt == c && t == c && l == c && r != c && ld != 0 && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }
            if (lt == c && t == c && l == c && r == c && ld != 0 && d != 0 && rd == 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[1].Tiles[Random.Range(0, wallTopRule.TileGroups[1].Tiles.Length)]); return; }

            //����� �����
            if (t == c && rt == c && l != c && r == c && d != 0 && rd != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[2].Tiles[Random.Range(0, wallTopRule.TileGroups[2].Tiles.Length)]); return; }

            //��� �����
            if (t == c && l != c && r != c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[3].Tiles[Random.Range(0, wallTopRule.TileGroups[3].Tiles.Length)]); return; }

            //��� ����� ������
            if (t != c && l != c && r != c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[4].Tiles[Random.Range(0, wallTopRule.TileGroups[4].Tiles.Length)]); return; }

            //������ ����
            if (t != c && l == c && r != c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[5].Tiles[Random.Range(0, wallTopRule.TileGroups[5].Tiles.Length)]); return; }

            //����� ����
            if (t != c && l != c && r == c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[6].Tiles[Random.Range(0, wallTopRule.TileGroups[6].Tiles.Length)]); return; }

            //������ ����
            if (t != c && l == c && r == c && d != 0) { wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[7].Tiles[Random.Range(0, wallTopRule.TileGroups[7].Tiles.Length)]); return; }

            //�������� ����
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
        /// ����������� ����� � ��������� ������ ����� � ����������� �� ������.
        /// </summary>
        /// <param name="x">���������� X</param>
        /// <param name="y">���������� Y</param>
        /// <param name="wall">������� �������</param>
        /// <param name="tile">��������� ������������ ��� ������������� �����</param>
        /// <param name="radius">������ ���������� ������ ������ ������������ �����. ������ ��������� ������� 1.</param>
        /// <param name="mode">false - ����� ����������, true - ����� ���������</param>
        public static void EditWallsAroundPoint(int x, int y, Tilemap wall, TileBlockBase tile, int radius, bool mode = false)
        {
            TileBehaviourRule wallSideRule = Singleton_TileLibrary.Instance.ReturnWallSideRuleByTag(tile.Tag);
            TileBehaviourRule wallTopRule = Singleton_TileLibrary.Instance.ReturnWallTopRuleByTag(tile.Tag);

            int c = 2;

            int lt = 0;
            int t = 0;
            int rt = 0;
            int l = 0;
            int r = 0;
            int ld = 0;
            int d = 0;
            int rd = 0;

            TileBlockBase checkTile = null;


            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y + 1));
            if (checkTile != null && tile.InvokeRule) lt = (int)tile.BlockType;
            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y + 1));
            if (checkTile != null && tile.InvokeRule) t = (int)tile.BlockType;
            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y + 1));
            if (checkTile != null && tile.InvokeRule) rt = (int)tile.BlockType;
            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y));
            if (checkTile != null && tile.InvokeRule) l = (int)tile.BlockType;
            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y));
            if (checkTile != null && tile.InvokeRule) r = (int)tile.BlockType;
            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y - 1));
            if (checkTile != null && tile.InvokeRule) ld = (int)tile.BlockType;
            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x, y - 1));
            if (checkTile != null && tile.InvokeRule) d = (int)tile.BlockType;
            checkTile = wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y - 1));
            if (checkTile != null && tile.InvokeRule) rd = (int)tile.BlockType;

            if (mode == false)
            {

                if (tile.BlockType == BlockType.TOP)
                {
                    bool check = true;

                    TileBlockBase bottomTile = (TileBlockBase)wall.GetTile(new Vector3Int(x, y - 1));
                    TileBlockBase topTile = (TileBlockBase)wall.GetTile(new Vector3Int(x, y + 1));
                    if (bottomTile != null && bottomTile.BlockType == BlockType.SIDE)
                    {
                        wall.SetTile(new Vector3Int(x, y - 1), null);
                        if (wall.GetTile(new Vector3Int(x, y - 1)) == null) d = 0;
                    }
                                        TileBehaviourRule oldSideRule = null;
                    if (topTile != null && tile.Tag != topTile.Tag && topTile.BlockType == BlockType.TOP)
                    {
                        oldSideRule = wallSideRule;
                        wallSideRule = Singleton_TileLibrary.Instance.ReturnWallSideRuleByTag(topTile.Tag);
                        if (wallSideRule != null) { }
                        else wallSideRule = oldSideRule;
                    }

                    if (topTile != null && topTile.BlockType == BlockType.SIDE)
                    {
                        wall.SetTile(new Vector3Int(x, y), null);
                        check = false;
                    }
                    //������ �����
                    if (check && t == c && l != 0 && r != 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                    else if (check && t == c && l != 0 && r == 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                    else if (check && t == c && l == 0 && r != 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                    else if (check && t == c && l == 0 && r == 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }
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
                TileBlockBase bottomTile = (TileBlockBase)wall.GetTile(new Vector3Int(x, y - 1));
                if (tile.BlockType == BlockType.TOP)
                {
                    TileBlockBase topTile = (TileBlockBase)wall.GetTile(new Vector3Int(x, y + 1));
                    if ((bottomTile == null && topTile == null) || (bottomTile == null && topTile != null && topTile.InvokeRule == true && topTile.BlockType == BlockType.SIDE) ||
                        (bottomTile != null && bottomTile.InvokeRule == false && topTile == null) || (bottomTile != null && bottomTile.InvokeRule == false && topTile != null && topTile.InvokeRule == true && topTile.BlockType == BlockType.SIDE))
                    {
                        wall.SetTile(new Vector3Int(x, y + 1), wallTopRule.TileGroups[0].Tiles[0]);
                        m = 0;
                    }


                    if (bottomTile == null && topTile != null && topTile.InvokeRule == false)
                    {
                        wall.SetTile(new Vector3Int(x, y), wallTopRule.TileGroups[0].Tiles[0]);
                        wall.SetTile(new Vector3Int(x, y - 1), wallTopRule.TileGroups[0].Tiles[0]);
                        //m = 0;
                    }


                    if (bottomTile != null && topTile!= null && topTile.BlockType == BlockType.SIDE && Singleton_SessionData.Instance.IsTop == false)
                    {
                        wall.SetTile(new Vector3Int(x, y + 1), wallTopRule.TileGroups[0].Tiles[0]);
                        d = 0;
                        t = c;
                        m = 0;
                    }
                    if (bottomTile != null && topTile != null && topTile.BlockType == BlockType.SIDE && Singleton_SessionData.Instance.IsTop == true)
                    {
                        m = 0;
                    }
                }

                if (tile.BlockType == BlockType.SIDE)
                {
                    if (bottomTile == null)
                    {
                        wall.SetTile(new Vector3Int(x, y - 1), wallTopRule.TileGroups[0].Tiles[0]);
                    }
                }
                bottomTile = (TileBlockBase)wall.GetTile(new Vector3Int(x, y - 1));
                if (bottomTile == null)
                {
                    if (wall.GetTile(new Vector3Int(x, y)) != null) t = (int)GetTileType(wall, x, y);
                    if (wall.GetTile(new Vector3Int(x - 1, y - 1)) != null) l = (int)GetTileType(wall, x - 1, y - m);
                    if (wall.GetTile(new Vector3Int(x + 1, y - 1)) != null) r = (int)GetTileType(wall, x + 1, y - m);
                    d = 0;
                }
                

                //������ �����
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
