using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScorgedEarth
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
        /// <param name="wallSideRule">������� �������� ����</param>
        /// <param name="wallTopRule">������� ������� ����</param>
        /// <param name="mode">0 - ������, 1 - ������ ��������, 2 - ������ �������</param>
        public static void PlaceEditedWallsAltRule(int x, int y, Tilemap wall, TileBehaviourRule wallSideRule, TileBehaviourRule wallTopRule, int mode = 0)
        {
            if (mode > 2 || mode < 0) { Debug.LogError("Alt Rule Generation mode set wrongly! Must use value between 0 and 2, inclusive."); return; }
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x, y, 0)) == null) return;
            int c = 2;

            int lt = 0;
            int t = 0;
            int rt = 0;
            int l = 0;
            int r = 0;
            int ld = 0;
            int d = 0;
            int rd = 0;


            if (wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y + 1)) != null) lt = (int)GetTileType(wall, x - 1, y + 1);
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x, y + 1)) != null) t = (int)GetTileType(wall, x, y + 1);
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y + 1)) != null) rt = (int)GetTileType(wall, x + 1, y + 1);
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y)) != null) l = (int)GetTileType(wall, x - 1, y);
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y)) != null) r = (int)GetTileType(wall, x + 1, y);
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x - 1, y - 1)) != null) ld = (int)GetTileType(wall, x - 1, y - 1);
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x, y - 1)) != null) d = (int)GetTileType(wall, x, y - 1);
            if (wall.GetTile<TileBlockBase>(new Vector3Int(x + 1, y - 1)) != null) rd = (int)GetTileType(wall, x + 1, y - 1);

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
        /// <summary>
        /// ����������� ����� � ��������� ������ ����� � ����������� �� ������.
        /// </summary>
        /// <param name="x">���������� X</param>
        /// <param name="y">���������� Y</param>
        /// <param name="wall">������� �������</param>
        /// <param name="wallSideRule">������� �������� ����</param>
        /// <param name="wallTopRule">������� ������� ����</param>
        /// <param name="starterType">��� ����� ������������ �����, ������ �������� ������� ����������. ������������ ���� Mode = 0</param>
        /// <param name="radius">������ ���������� ������ ������ ������������ �����. ������ ��������� ������� 1.</param>
        /// <param name="mode">false - ����� ����������, true - ����� ���������</param>
        public static void EditWallsAroundPoint(int x, int y, Tilemap wall, TileBehaviourRule wallSideRule, TileBehaviourRule wallTopRule, BlockType starterType, int radius, bool mode = false)
        {
            int c = 2;

            int lt = 0;
            int t = 0;
            int rt = 0;
            int l = 0;
            int r = 0;
            int ld = 0;
            int d = 0;
            int rd = 0;

            if (wall.GetTile(new Vector3Int(x - 1, y + 1)) != null) lt = (int)GetTileType(wall, x - 1, y + 1);
            if (wall.GetTile(new Vector3Int(x, y + 1)) != null) t = (int)GetTileType(wall, x, y + 1);
            if (wall.GetTile(new Vector3Int(x + 1, y + 1)) != null) rt = (int)GetTileType(wall, x + 1, y + 1);
            if (wall.GetTile(new Vector3Int(x - 1, y)) != null) l = (int)GetTileType(wall, x - 1, y);
            if (wall.GetTile(new Vector3Int(x + 1, y)) != null) r = (int)GetTileType(wall, x + 1, y);
            if (wall.GetTile(new Vector3Int(x - 1, y - 1)) != null) ld = (int)GetTileType(wall, x - 1, y - 1);
            if (wall.GetTile(new Vector3Int(x, y - 1)) != null) d = (int)GetTileType(wall, x, y - 1);
            if (wall.GetTile(new Vector3Int(x + 1, y - 1)) != null) rd = (int)GetTileType(wall, x + 1, y - 1);

            if (mode == false)
            {

                if (starterType == BlockType.TOP)
                {
                    TileBlockBase tl = (TileBlockBase)wall.GetTile(new Vector3Int(x, y - 1));
                    if (tl != null && tl.BlockType == BlockType.SIDE)
                    {
                        wall.SetTile(new Vector3Int(x, y - 1), null);
                        if (wall.GetTile(new Vector3Int(x, y - 1)) == null) d = 0;
                    }
                    //������ �����
                    if (t == c && l != 0 && r != 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                    else if (t == c && l != 0 && r == 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                    else if (t == c && l == 0 && r != 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                    else if (t == c && l == 0 && r == 0 && d != 1) { wall.SetTile(new Vector3Int(x, y), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }
                }

                if (starterType == BlockType.SIDE)
                {
                    TileBlockBase tl = (TileBlockBase)wall.GetTile(new Vector3Int(x, y + 2));
                    if (tl == null || tl.BlockType == BlockType.SIDE)
                    {
                        wall.SetTile(new Vector3Int(x, y + 1), null);
                        if (wall.GetTile(new Vector3Int(x, y - 1)) != null) d = (int)GetTileType(wall, x, y - 1);
                    }
                    else if (t == c && l != 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                    else if (t == c && l != 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                    else if (t == c && l == 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                    else if (t == c && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y + 1), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }
                }
               
            }

            if (mode == true)
            {
                TileBlockBase tl = (TileBlockBase)wall.GetTile(new Vector3Int(x, y - 1));
                if (starterType == BlockType.FLOOR)
                {
                    if (tl == null)
                    {
                        wall.SetTile(new Vector3Int(x, y - 1), wallTopRule.TileGroups[0].Tiles[0]);
                    }
                }

                if (starterType == BlockType.SIDE)
                {
                    if (tl == null)
                    {
                        wall.SetTile(new Vector3Int(x, y - 1), wallTopRule.TileGroups[0].Tiles[0]);
                    }
                }

                if (tl == null)
                {
                    if (wall.GetTile(new Vector3Int(x, y)) != null) t = (int)GetTileType(wall, x, y);
                    if (wall.GetTile(new Vector3Int(x - 1, y - 1)) != null) l = (int)GetTileType(wall, x - 1, y - 1);
                    if (wall.GetTile(new Vector3Int(x + 1, y - 1)) != null) r = (int)GetTileType(wall, x + 1, y - 1);
                    d = 0;
                }
                

                //������ �����
                if (t == c && l != 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - 1), wallSideRule.TileGroups[0].Tiles[Random.Range(0, wallSideRule.TileGroups[0].Tiles.Length)]); }
                else if (t == c && l != 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - 1), wallSideRule.TileGroups[1].Tiles[Random.Range(0, wallSideRule.TileGroups[2].Tiles.Length)]); }
                else if (t == c && l == 0 && r != 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - 1), wallSideRule.TileGroups[2].Tiles[Random.Range(0, wallSideRule.TileGroups[1].Tiles.Length)]); }
                else if (t == c && l == 0 && r == 0 && d == 0) { wall.SetTile(new Vector3Int(x, y - 1), wallSideRule.TileGroups[3].Tiles[Random.Range(0, wallSideRule.TileGroups[3].Tiles.Length)]); }

            }

            int md = 0;
            if (mode) md = 1;
            if (radius <= 0) radius = 1;

            for (int i = x - radius; i <= radius + x; i++)
            {
                for (int j = y - radius; j <= radius + y; j++)
                {
                    if (i == j && i==x && j == y && !mode) continue;
                    PlaceEditedWallsAltRule(i, j, wall, wallSideRule, wallTopRule, 1);
                }
            }
            for (int i = x - radius; i <= radius + x; i++)
            {
                for (int j = y - radius - md; j <= radius + y; j++)
                {
                    if (i == j && i == x && j == y && !mode) continue;
                    PlaceEditedWallsAltRule(i, j, wall, wallSideRule, wallTopRule, 2);
                }
            }
        }
    }
}
