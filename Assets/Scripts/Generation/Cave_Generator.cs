using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

namespace DecayingEarth
{
    public class Cave_Generator : MonoSingleton<Cave_Generator>
    {
        /// <summary>
        /// X и Y размер генерируемой пещеры
        /// </summary>
        private int m_CaveXSize;
        private int m_CaveYSize;

        /// <summary>
        /// Ссылки на целевые тайлмапы - пол, стены и руды.
        /// </summary>
        [SerializeField] private Tilemap m_FloorTilemap;
        
        [SerializeField] private Tilemap m_WallsTilemap;
        
        [SerializeField] private Tilemap m_OresTilemap;

        private TileBehaviourRule[] m_FloorRule;
        public TileBehaviourRule[] FloorRule => m_FloorRule;

        private TileBehaviourRule[] m_WallFrontRule;
        public TileBehaviourRule[] wallSideRule => m_WallFrontRule;

        private TileBehaviourRule[] m_WallTopRule;
        public TileBehaviourRule[] WallTopRule => m_WallTopRule;

        /// <summary>
        /// Генерация мира по-правилам. Если не в редакторе - игнорируется.
        /// </summary>
        [SerializeField] private WorldGenerationRules m_WorldRule;

        /// <summary>
        /// Процент заполнения карты при первой генерации. Оптимально - 52%
        /// </summary>
        private int m_MapFillPercent;

        /// <summary>
        /// Количечство итераций через клеточный автомат. Больше итераций - более "гладкие" стены.
        /// </summary>
        private int m_NumberOfCAIterations;

        
        [SerializeField] private int m_Seed;
        /// <summary>
        /// Семя генерации. Если пустое - выбирается случайное.
        /// </summary>
        public int Seed => m_Seed;

        /// <summary>
        /// Список генерируемых руд.
        /// </summary>
        private TileGroup_Ore[] m_Ores;

        /// <summary>
        /// Множитель руд.
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
            m_MapFillPercent = rules.m_MapFillPercent;
            m_NumberOfCAIterations = rules.m_NumberOfCAIterations;
            m_Ores = rules.Ores;
            m_OreMultiplier = rules.OreGenerationMultiplier;
        }

        public void SetSeed(int number) => m_Seed = number;

        [ContextMenu(nameof(GenerateMap))]
        public void GenerateMap()
        {

#if UNITY_EDITOR
            SetWorldGenerationSettings(m_WorldRule);
            TileBlockBase tile = m_WallsTilemap.GetTile<TileBlockBase>(new Vector3Int(4, 4));
            if (tile == null) Debug.Log(BlockType.FLOOR);
            else Debug.Log(tile.BlockType);
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
            GenerateFloor(m_FloorTilemap, m_FloorRule[0]);
            GenerateRandomWallPattern(m_WallsTilemap, m_WallTopRule[0]);

            for (int i = 0; i < m_NumberOfCAIterations; i++) CellAutomataIteration(m_WallsTilemap, m_WallTopRule[0]);

            PlaceEditedWalls(m_WallsTilemap);

            if (m_Ores.Length > 0) for (int i = 0; i < m_Ores.Length; i++) GenerateOresByTypeCountAndSize(m_Ores[i], m_OresTilemap, m_WallsTilemap);
            else Debug.Log("Warning: No ores to generate!");
        }

        /// <summary>
        /// Генерирует стены на выбранном тайлмапе по определенным правилам.
        /// </summary>
        /// <param name="floor">Целевой Тайлмап</param>
        /// <param name="floorRule">Правило укладки пола</param>
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
        /// Случайная генерация стен в разных точках на выбранном тайлмапе. Зависит от параметра Map Fill Percent
        /// </summary>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="wallTopRule">Правило верхних стен</param>
        private void GenerateRandomWallPattern(Tilemap wall, TileBehaviourRule wallTopRule)
        {
            for (int i = -1 - m_XOffset; i <= m_CaveXSize - m_XOffset; i++)
            {

                for (int j = -1 - m_YOffset; j <= m_CaveYSize - m_YOffset; j++)
                {
                    if (i <= 0 - m_XOffset || i >= m_CaveXSize - 1 - m_XOffset || j <= 0 - m_YOffset || j >= m_CaveYSize - 1 - m_YOffset)
                    {
                        wall.SetTile(new Vector3Int(i, j, 0), wallTopRule.TileGroups[0].Tiles[0]);
                        continue;
                    }

                    int x = Random.Range(0, 100);
                    if (x < m_MapFillPercent)
                    {
                        wall.SetTile(new Vector3Int(i, j, 0), wallTopRule.TileGroups[0].Tiles[0]);
                    }
                    else continue;
                }
            }
        }

        /// <summary>
        /// Итерирует стены клеточным автоматом один раз на выбранном тайлмапе.
        /// </summary>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="wallTopRule">Правило верхних стен</param>
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
        /// Алгоритм покраски стен на выбранном тайлмапе по заданным правилам. Нужно правило передних стен и верхних.
        /// </summary>
        /// <param name="wall">Целевой Тайлмап</param>
        private void PlaceEditedWalls(Tilemap wall)
        {
            StartCoroutine(Cycle(m_CaveXSize, m_CaveYSize, m_XOffset, m_YOffset, wall));
        }

        IEnumerator Cycle(int CaveX, int CaveY, int xo, int yo, Tilemap wall)
        {
            Cave_Generator cg = null;

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
        /// Генерирует руду с заданным правилом размером и шансом.
        /// </summary>
        /// <param name="ore">Группа генерируемой руды</param>
        /// <param name="oremap">Тайлмап руды</param>
        /// <param name="wallmap">Тайлмап стен</param>
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

                        if (tile != null && tile.BlockType == BlockType.TOP && oremap.GetTile(new Vector3Int(x, y, 0)) == null)
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

        

    }
}
