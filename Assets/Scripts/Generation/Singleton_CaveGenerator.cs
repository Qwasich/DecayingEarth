using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using static DecayingEarth.WorldGenerationRules;

namespace DecayingEarth
{
    public class Singleton_CaveGenerator : MonoSingleton<Singleton_CaveGenerator>
    {
        /// <summary>
        /// X � Y ������ ������������ ������
        /// </summary>
        private int m_CaveXSize;
        private int m_CaveYSize;

        /// <summary>
        /// ������ �� ������� �������� - ���, �����, ���� � ����.
        /// </summary>
        [SerializeField] private Tilemap m_FloorTilemap;

        [SerializeField] private Tilemap m_FloorDetsTilemap;

        [SerializeField] private Tilemap m_WallsTilemap;
        
        [SerializeField] private Tilemap m_OresTilemap;


        private TileBehaviourRule[] m_FloorRule;
        public TileBehaviourRule[] FloorRule => m_FloorRule;

        private TileBehaviourRule[] m_WallFrontRule;
        public TileBehaviourRule[] WallFrontRule => m_WallFrontRule;

        private TileBehaviourRule[] m_WallTopRule;
        public TileBehaviourRule[] WallTopRule => m_WallTopRule;

        private FeatureSlot[] m_FloorFeatureRule;
        public FeatureSlot[] FloorFeatureRule => m_FloorFeatureRule;

        private TileBlockGrowablle[] m_AllowedFoliage;
        public TileBlockGrowablle[] AllowedFoliage => m_AllowedFoliage;

        /// <summary>
        /// ��������� ���� ��-��������. ���� �� � ��������� - ������������.
        /// </summary>
        [SerializeField] private WorldGenerationRules m_WorldRule;

        /// <summary>
        /// ������� ���������� ����� ��� ������ ���������. ���������� - 52%
        /// </summary>
        private int m_MapFillPercent;

        /// <summary>
        /// ����������� �������� ����� ��������� �������. ������ �������� - ����� "�������" �����.
        /// </summary>
        private int m_NumberOfCAIterations;

        
        [SerializeField] private int m_Seed;
        /// <summary>
        /// ���� ���������. ���� ������ - ���������� ���������.
        /// </summary>
        public int Seed => m_Seed;

        /// <summary>
        /// ������ ������������ ���.
        /// </summary>
        private TileGroup_Ore[] m_Ores;

        /// <summary>
        /// ��������� ���.
        /// </summary>
        private int m_OreMultiplier;

        private int m_XOffset;
        private int m_YOffset;


        protected override void Awake()
        {
            SetWorldGenerationSettings(m_WorldRule);
            base.Awake();
            
        }

        public void SetWorldGenerationSettings(WorldGenerationRules rules)
        {
            m_CaveXSize = rules.m_CaveXSize;
            m_CaveYSize = rules.m_CaveYSize;
            m_FloorRule = rules.m_FloorRule;
            m_WallFrontRule = rules.m_WallFrontRule;
            m_WallTopRule = rules.m_WallTopRule;
            m_FloorFeatureRule = rules.m_Features;
            m_MapFillPercent = rules.m_MapFillPercent;
            m_NumberOfCAIterations = rules.m_NumberOfCAIterations;
            m_Ores = rules.Ores;
            m_OreMultiplier = rules.OreGenerationMultiplier;
            m_AllowedFoliage = rules.m_AllowedFoliage;
        }

        public void SetSeed(int number) => m_Seed = number;

        [ContextMenu(nameof(GenerateMap))]
        public void GenerateMap()
        {

#if UNITY_EDITOR
            SetWorldGenerationSettings(m_WorldRule);
#endif
            m_XOffset = Mathf.FloorToInt(m_CaveXSize / 2);
            m_YOffset = Mathf.FloorToInt(m_CaveYSize / 2);
            if (m_FloorRule == null || m_WallFrontRule == null || m_WallTopRule == null || m_CaveXSize <= 0 || m_CaveYSize <= 0) { Debug.LogError("Error: Rules aren't set properly"); return; } 
            if (m_FloorTilemap == null || m_WallsTilemap == null || m_OresTilemap == null) { Debug.LogError("Error: Target tilemaps are not set"); return; } 
            if (m_MapFillPercent == 0 || m_MapFillPercent == 100) { Debug.Log("Warning: Map fill percent " + m_MapFillPercent +"!"); return; }
            if (m_NumberOfCAIterations <= 0) { Debug.Log("Warning: Iteration count set to " + m_NumberOfCAIterations + "!"); return; }
            if (m_Seed != 0) Random.InitState(m_Seed);
            m_FloorTilemap.ClearAllTiles();
            m_WallsTilemap.ClearAllTiles();
            m_OresTilemap.ClearAllTiles();
            m_FloorDetsTilemap.ClearAllTiles();
            GenerateFloor(m_FloorTilemap, m_FloorRule[0]);
            GenerateRandomWallPattern(m_WallsTilemap, m_WallTopRule[0]);

            for (int i = 0; i < m_NumberOfCAIterations; i++) CellAutomataIteration(m_WallsTilemap, m_WallTopRule[0]);

            for (int i = 1; i < m_WallTopRule.Length; i++)
            {
                if (m_WallTopRule[i].Tag == "BorderBlock") continue;
                GenerateBlocksByPerlinNoise(Random.Range(0, 100), m_WallsTilemap, m_WallTopRule[i], 0.65f);
            }

            CheckForWallGenerationErrors(m_WallsTilemap);

            GenerateStartingRoom(m_WallsTilemap, new Vector2Int(2, 2));

            PlaceEditedWalls(m_WallsTilemap);

            if (m_Ores.Length > 0) for (int i = 0; i < m_Ores.Length; i++) GenerateOresByTypeCountAndSize(m_Ores[i], m_OresTilemap, m_WallsTilemap);
            else Debug.Log("Warning: No ores to generate!");

            if (m_FloorFeatureRule.Length > 0) for (int i = 0; i < m_FloorFeatureRule.Length; i++) PlaceFeatureInRandomRoom(m_FloorFeatureRule[i], m_WallsTilemap, m_FloorDetsTilemap);
            else Debug.Log("Warning: No floor features to generate!");
        }

        /// <summary>
        /// ���������� ����� �� ��������� �������� �� ������������ ��������.
        /// </summary>
        /// <param name="floor">������� �������</param>
        /// <param name="floorRule">������� ������� ����</param>
        private void GenerateFloor(Tilemap floor, TileBehaviourRule floorRule)
        {
            for (int i = 0 - m_XOffset; i < m_CaveXSize - m_XOffset; i++)
            {
                for (int j = 0 - m_YOffset; j < m_CaveYSize - m_YOffset; j++)
                {
                    floor.SetTile(new Vector3Int(i, j, 0), floorRule.TileGroups[0].Tiles[Random.Range(0, floorRule.TileGroups[0].Tiles.Length)]);
                }
            }
        }

        /// <summary>
        /// ��������� ��������� ���� � ������ ������ �� ��������� ��������. ������� �� ��������� Map Fill Percent
        /// </summary>
        /// <param name="wall">������� �������</param>
        /// <param name="wallTopRule">������� ������� ����</param>
        private void GenerateRandomWallPattern(Tilemap wall, TileBehaviourRule wallTopRule)
        {
            for (int i = -2 - m_XOffset; i < 2 + m_CaveXSize - m_XOffset; i++)
            {

                for (int j = -2 - m_YOffset; j < 2 + m_CaveYSize - m_YOffset; j++)
                {
                    if (i <= 0 - m_XOffset || i >= m_CaveXSize - 2 - m_XOffset || j <= 0 - m_YOffset || j >= m_CaveYSize - 2 - m_YOffset)
                    {
                        wall.SetTile(new Vector3Int(i, j, 0), GetTopWallRuleByTag("BorderBlock").TileGroups[0].Tiles[0]);
                        continue;
                    }

                    int x = Random.Range(0, 100);
                    if (x < m_MapFillPercent)
                    {
                        wall.SetTile(new Vector3Int(i, j, 0), GetTopWallRuleByTag("StoneRaw").TileGroups[0].Tiles[0]);
                    }
                    else continue;
                }
            }
        }

        /// <summary>
        /// ���������� ������� �� ����
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>������� �������� �����</returns>
        private TileBehaviourRule GetFrontWallRuleByTag(string tag)
        {
            for (int i = 0; i< m_WallFrontRule.Length; i++)
            {
                if (tag == m_WallFrontRule[i].Tag) return m_WallFrontRule[i];
            }

            Debug.LogError("Tag side isn't found, returning default");
            return m_WallFrontRule[0];
        }
        /// <summary>
        /// ���������� ������� �� ����
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>������� ������� �����</returns>
        private TileBehaviourRule GetTopWallRuleByTag(string tag)
        {
            for (int i = 0; i < m_WallTopRule.Length; i++)
            {
                if (tag == m_WallTopRule[i].Tag) return m_WallTopRule[i];
            }

            Debug.LogError("Tag top isn't found, returning default");
            return m_WallTopRule[0];
        }
        /// <summary>
        /// ���������� ������� �� ����
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>������� ����</returns>
        private TileBehaviourRule GetFloorRuleByTag(string tag)
        {
            for (int i = 0; i < m_FloorRule.Length; i++)
            {
                if (tag == m_FloorRule[i].Tag) return m_FloorRule[i];
            }

            Debug.LogError("Tag floor isn't found, returning default");
            return m_FloorRule[0];
        }

        /// <summary>
        /// ���������� ������� �� ����
        /// </summary>
        /// <param name="tag">��� ������</param>
        /// <returns>������� ���� ����</returns>
        private TileBehaviourRule GetFloorFeatureByTag(string tag)
        {
            for (int i = 0; i < m_FloorFeatureRule.Length; i++)
            {
                if (tag == m_FloorFeatureRule[i].TileRule.Tag) return m_FloorFeatureRule[i].TileRule;
            }

            Debug.LogError("Tag floor feature isn't found, returning default");
            return m_FloorFeatureRule[0].TileRule;
        }

        /// <summary>
        /// ��������� ����� ��������� ��������� ���� ��� �� ��������� ��������.
        /// </summary>
        /// <param name="wall">������� �������</param>
        /// <param name="wallTopRule">������� ������� ����</param>
        private void CellAutomataIteration(Tilemap wall, TileBehaviourRule wallTopRule)
        {
            for (int i = 1 - m_XOffset; i < m_CaveXSize - 1 - m_XOffset; i++)
            {
                for (int j = 1 - m_YOffset; j < m_CaveYSize - 1 - m_YOffset; j++)
                {
                    int counter = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int t = j - 1; t <= j + 1; t++)
                        {
                            if (k == i && t == j) continue;
                            if (wall.GetTile(new Vector3Int(k,t,0)) != null) counter++;
                        }
                    }

                    if (counter > 4) wall.SetTile(new Vector3Int(i, j, 0), wallTopRule.TileGroups[0].Tiles[0]);
                    if (counter < 4) wall.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
        }

        /// <summary>
        /// �������� �������� ���� �� ��������� �������� �� �������� ��������. ����� ������� �������� ���� � �������.
        /// </summary>
        /// <param name="wall">������� �������</param>
        private void PlaceEditedWalls(Tilemap wall)
        {
            StartCoroutine(Cycle(m_CaveXSize, m_CaveYSize, m_XOffset, m_YOffset, wall));
        }

        IEnumerator Cycle(int CaveX, int CaveY, int xo, int yo, Tilemap wall)
        {
            Singleton_CaveGenerator cg = null;

#if UNITY_EDITOR
            cg = this;
#endif
            for (int i = 0 - xo; i < CaveX - xo; i++)
            {
                for (int j = 0 - yo; j < CaveY - yo; j++)
                {
                    WorldShaper.PlaceEditedWallsAltRule(i, j, wall, 0, cg);
                }
            }
            yield return null;
        }

        /// <summary>
        /// ���������� ���� � �������� �������� �������� � ������.
        /// </summary>
        /// <param name="ore">������ ������������ ����</param>
        /// <param name="oremap">������� ����</param>
        /// <param name="wallmap">������� ����</param>
        private void GenerateOresByTypeCountAndSize(TileGroup_Ore ore, Tilemap oremap, Tilemap wallmap)
        {
            if (ore.Tiles.Length == 0) { Debug.LogError("Error: No ore tiles set to " + ore.GroupName + " tilemap."); return; }

            int rarity = Random.Range(ore.RarityMin, ore.RarityMax + 1);
            rarity += rarity * m_OreMultiplier;

            while (rarity > 0)
            {
                int orecount = ore.VeinSize;
                int xrand = Random.Range(0 - m_XOffset, m_CaveXSize - m_XOffset);
                int yrand = Random.Range(0 - m_YOffset, m_CaveYSize - m_YOffset);
                TileBlockBase tile = wallmap.GetTile<TileBlockBase>(new Vector3Int(xrand, yrand, 0));
                if (tile == null) continue;
                if (tile.BlockType != BlockType.TOP) continue;
                oremap.SetTile(new Vector3Int(xrand, yrand, 0), ore.Tiles[Random.Range(0,ore.Tiles.Length)]);
                orecount--;
                while (orecount > 0)
                {
                    int x = 0;
                    int y = 0;
                    List<char> direction = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
                    while (direction.Count > 0)
                    {
                        switch (direction[Random.Range(0, direction.Count)])
                        {
                            case 'a': x = -1; y = 1; direction.Remove('a'); break;
                            case 'b': x = 0; y = 1; direction.Remove('b'); break;
                            case 'c': x = 1; y = 1; direction.Remove('c'); break;
                            case 'd': x = -1; y = 0; direction.Remove('d'); break;
                            case 'e': x = 1; y = 0; direction.Remove('e'); break;
                            case 'f': x = -1; y = -1; direction.Remove('f'); break;
                            case 'g': x = 0; y = -1; direction.Remove('g'); break;
                            case 'h': x = 1; y = -1; direction.Remove('h'); break;
                        }
                        x += xrand;
                        y += yrand;

                        tile = wallmap.GetTile<TileBlockBase>(new Vector3Int(x, y, 0));

                        if (tile != null && tile.BlockType == BlockType.TOP && tile.Tag != "BorderBlock" && oremap.GetTile(new Vector3Int(x, y, 0)) == null)
                        {
                            oremap.SetTile(new Vector3Int(xrand, yrand, 0), ore.Tiles[Random.Range(0, ore.Tiles.Length)]);
                            orecount--;
                            xrand = x;
                            yrand = y;
                            break;
                        }
                        if (direction.Count == 0) orecount = 0;
                    }
                }
                rarity--;
            }
        }

        /// <summary>
        /// ������ ����� �� ���� �������
        /// </summary>
        /// <param name="seed">���� ��������� ����, �� ���� ����� ���� �������</param>
        /// <param name="wallmap">������� �������</param>
        /// <param name="wallTopRule">������� ������, ������� �� ����� ���������</param>
        /// <param name="treshold">��������������� � ����, �� 0.1 �� 1 ������������. ���� �������� ���� ��������� - ����� ��������� ����.</param>
        private void GenerateBlocksByPerlinNoise(float seed, Tilemap wallmap, TileBehaviourRule wallTopRule, float treshold)
        {
            if (treshold > 1) treshold = 1;
            if (treshold < 0.1) treshold = 0.1f;

            seed *= treshold;
            StartCoroutine(PerlinPlacer(m_CaveXSize, m_CaveYSize, m_XOffset, m_YOffset, wallmap, (TileBlockBase)wallTopRule.TileGroups[0].Tiles[0],seed, treshold));
        }

        IEnumerator PerlinPlacer(int CaveX, int CaveY, int xo, int yo, Tilemap wallmap, TileBlockBase tile, float seed, float treshold)
        {
            float modx = 1f / CaveX;
            float mody = 1f / CaveY;

            float mods = 1f / seed;


            for (int i = 1 - xo; i < CaveX - xo - 1; i++)
            {
                for (int j = 1 - yo; j < CaveY - yo - 1; j++)
                {
                    TileBlockBase tiles = wallmap.GetTile<TileBlockBase>(new Vector3Int(i, j, 0));
                    if (tiles == null) continue;
                    float p = Mathf.PerlinNoise((i * modx + mods) * (treshold / (modx * 10)), (j * mody + mods) * (treshold / (mody * 10)));
                    if (p >= treshold) wallmap.SetTile(new Vector3Int(i, j, 0), tile);


                }
            }
            yield return null;
        }

        /// <summary>
        /// ��������� �����, ����� ������������ �� � �������. �����, ����� ��������� ���������� ���, ��� ����� �� ������� �� �����-�� ������� ����� ���������� �� ���� ������.
        /// </summary>
        /// <param name="tilemap">������� �������</param>
        private void CheckForWallGenerationErrors(Tilemap tilemap)
        {
            for (int i = 1 - m_XOffset; i < m_CaveXSize - m_XOffset - 1; i++)
            {
                for (int j = 1 - m_YOffset; j < m_CaveYSize - m_YOffset - 1; j++)
                {
                    TileBlockBase tile = tilemap.GetTile<TileBlockBase>(new Vector3Int(i, j, 0));
                    if (tile == null) continue;
                    TileBlockBase toptile = tilemap.GetTile<TileBlockBase>(new Vector3Int(i, j + 1, 0));
                    if (toptile == null) continue;
                    TileBlockBase bottomtile = tilemap.GetTile<TileBlockBase>(new Vector3Int(i, j - 1, 0));

                    if (bottomtile == null && toptile.Tag != tile.Tag) tilemap.SetTile(new Vector3Int(i, j, 0), toptile);
                }
            }
        }

        /// <summary>
        /// ������ ���� ���� �� �����, � ��������� ��������
        /// </summary>
        /// <param name="Feature">����, ������� ������</param>
        /// <param name="wallTilemap">������� ����</param>
        /// <param name="featureFloorTilemap">������� ��� ����</param>
        private void PlaceFeatureInRandomRoom(FeatureSlot Feature, Tilemap wallTilemap, Tilemap featureFloorTilemap)
        {
            if (featureFloorTilemap == null || wallTilemap == null) return;
            if (Feature.TileRule == null) return;

            int mult = Feature.GenerationMultiplier;
            if (mult < 1) mult = 1;

            int max = Feature.MaximalAmount * mult;
            int min = Feature.MinimalAmount * mult;

            int actual = Random.Range(min, max + 1);

            int maxIter = 0;

            for (int i = 0; i < actual; i++)
            {
                if (maxIter > (m_CaveXSize + m_CaveYSize) * 2)
                {
                    Debug.Log(" Flood fill exceeded safe amount of iterations, aborting");
                    break;
                }
                Vector3Int rCrd = new Vector3Int(Random.Range(1 - m_XOffset, m_CaveXSize - m_XOffset), Random.Range(1 - m_YOffset, m_CaveYSize - m_YOffset));
                if (FindEmptyRoomByFlood(rCrd, wallTilemap, featureFloorTilemap, Feature)) continue;
                i--;
                maxIter++;
            }    

        }

        // private List<Vector3Int> m_LatestRoomFloodCoordinates;

        /// <summary>
        /// �������� ���������� ��������
        /// </summary>
        /// <param name="stCrd"><����������� ����������/param>
        /// <param name="wallTilemap">������� ������� ����</param>
        /// <param name="featureFloorTilemap">������� ������� ��� ����</param>
        /// <param name="slot">����, ������� ������</param>
        /// <returns></returns>
        private bool FindEmptyRoomByFlood(Vector3Int stCrd, Tilemap wallTilemap, Tilemap featureFloorTilemap, FeatureSlot slot)
        {
            if (wallTilemap.GetTile(stCrd) != null) return false;
            if (featureFloorTilemap.GetTile(stCrd) != null) return false;

            int r = Random.Range(0, slot.TileRule.TileGroups.Length * 3);

            if (r < slot.TileRule.TileGroups.Length * 2)
            {
                featureFloorTilemap.SetTile(stCrd, slot.TileRule.TileGroups[0].Tiles[Random.Range(0, slot.TileRule.TileGroups[0].Tiles.Length)]);
                TryPlaceFoliage(stCrd);
            }
            else featureFloorTilemap.SetTile(stCrd, slot.TileRule.TileGroups[Random.Range(1, slot.TileRule.TileGroups.Length)].Tiles[Random.Range(0, slot.TileRule.TileGroups[0].Tiles.Length)]);

            //m_LatestRoomFloodCoordinates.Add(stCrd);

            FindEmptyRoomByFlood(new Vector3Int(stCrd.x + 1, stCrd.y), wallTilemap, featureFloorTilemap, slot);
            FindEmptyRoomByFlood(new Vector3Int(stCrd.x, stCrd.y + 1), wallTilemap, featureFloorTilemap, slot);
            FindEmptyRoomByFlood(new Vector3Int(stCrd.x - 1, stCrd.y), wallTilemap, featureFloorTilemap, slot);
            FindEmptyRoomByFlood(new Vector3Int(stCrd.x, stCrd.y - 1), wallTilemap, featureFloorTilemap, slot);

            return true;
        }

        /// <summary>
        /// ���������� ��������� �������
        /// </summary>
        /// <param name="wallmap"></param>
        private void GenerateStartingRoom(Tilemap wallmap, Vector2Int size)
        {
            for (int i = -1 - size.x / 2; i <= size.x / 2; i++)
            {
                for (int j = -1 - size.y / 2; j <= size.y / 2; j++)
                {
                    wallmap.SetTile(new Vector3Int(i, j), null);
                }
            }
        }

        private void TryPlaceFoliage(Vector3Int coord)
        {
            Debug.Log("Reached");
            TileBlockGrowablle tile = m_AllowedFoliage[Random.Range(0, m_AllowedFoliage.Length)];
            if (tile == null) return;
            Debug.Log("Tile Set");
            int chance = Random.Range(0, tile.GrowthChance * 10);
            if (chance == 0) m_WallsTilemap.SetTile(coord, tile);

            Debug.Log("Tile Placed");
        }

    }
}
