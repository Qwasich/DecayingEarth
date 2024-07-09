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
        /// X и Y размер генерируемой пещеры
        /// </summary>
        private int m_CaveXSize;
        private int m_CaveYSize;

        /// <summary>
        /// Ссылки на целевые тайлмапы - пол, стены, руды и фичи.
        /// </summary>
        [SerializeField] private Tilemap m_FloorTilemap;

        [SerializeField] private Tilemap m_FloorDetsTilemap;

        [SerializeField] private Tilemap m_WallsTilemap;
        
        [SerializeField] private Tilemap m_OresTilemap;

        private TileGroup[] m_TileRule;
        public TileGroup[] TileRule => m_TileRule;

        private FeatureSlot[] m_FloorFeatureRule;
        public FeatureSlot[] FloorFeatureRule => m_FloorFeatureRule;

        private TileBlockGrowablle[] m_AllowedFoliage;
        public TileBlockGrowablle[] AllowedFoliage => m_AllowedFoliage;

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
            m_TileRule = rules.m_TileRule;
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
            if (m_TileRule == null || m_CaveXSize <= 0 || m_CaveYSize <= 0) { Debug.LogError("Error: Rules aren't set properly"); return; } 
            if (m_FloorTilemap == null || m_WallsTilemap == null || m_OresTilemap == null) { Debug.LogError("Error: Target tilemaps are not set"); return; } 
            if (m_MapFillPercent == 0 || m_MapFillPercent == 100) { Debug.Log("Warning: Map fill percent " + m_MapFillPercent +"!"); return; }
            if (m_NumberOfCAIterations <= 0) { Debug.Log("Warning: Iteration count set to " + m_NumberOfCAIterations + "!"); return; }
            if (m_Seed != 0) Random.InitState(m_Seed);
            m_FloorTilemap.ClearAllTiles();
            m_WallsTilemap.ClearAllTiles();
            m_OresTilemap.ClearAllTiles();
            m_FloorDetsTilemap.ClearAllTiles();
            GenerateFloor(m_FloorTilemap, m_TileRule[0]);
            GenerateRandomWallPattern(m_WallsTilemap, m_TileRule[0]);

            for (int i = 0; i < m_NumberOfCAIterations; i++) CellAutomataIteration(m_WallsTilemap, m_TileRule[0]);

            for (int i = 1; i < m_TileRule.Length; i++)
            {
                if (m_TileRule[i].BlockTag == "BorderBlock") continue;
                GenerateBlocksByPerlinNoise(Random.Range(0, 100), m_WallsTilemap, m_TileRule[i], 0.65f);
            }


            GenerateStartingRoom(m_WallsTilemap, new Vector2Int(2, 2));

            PlaceEditedWalls(m_WallsTilemap);

            if (m_Ores.Length > 0) for (int i = 0; i < m_Ores.Length; i++) GenerateOresByTypeCountAndSize(m_Ores[i], m_OresTilemap, m_WallsTilemap);
            else Debug.Log("Warning: No ores to generate!");

            if (m_FloorFeatureRule.Length > 0 && m_FloorFeatureRule != null) for (int i = 0; i < m_FloorFeatureRule.Length; i++) PlaceFeatureInRandomRoom(m_FloorFeatureRule[i], m_WallsTilemap, m_FloorDetsTilemap);
            else Debug.Log("Warning: No floor features to generate!");
        }

        /// <summary>
        /// Генерирует стены на выбранном тайлмапе по определенным правилам.
        /// </summary>
        /// <param name="floor">Целевой Тайлмап</param>
        /// <param name="tileGroup">Группа, откуда будем брать пол</param>
        private void GenerateFloor(Tilemap floor, TileGroup tileGroup)
        {
            for (int i = 0 - m_XOffset; i < m_CaveXSize - m_XOffset; i++)
            {
                for (int j = 0 - m_YOffset; j < m_CaveYSize - m_YOffset; j++)
                {
                    floor.SetTile(new Vector3Int(i, j, 0), tileGroup.Tiles[Random.Range(25, 33)]);
                }
            }
        }

        /// <summary>
        /// Случайная генерация стен в разных точках на выбранном тайлмапе. Зависит от параметра Map Fill Percent
        /// </summary>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="tileGroup">Группа тайлов</param>
        private void GenerateRandomWallPattern(Tilemap wall, TileGroup tileGroup)
        {
            for (int i = -2 - m_XOffset; i < 2 + m_CaveXSize - m_XOffset; i++)
            {

                for (int j = -2 - m_YOffset; j < 2 + m_CaveYSize - m_YOffset; j++)
                {
                    //TODO - перенести на Async или Ienumerator
                    if (i <= 0 - m_XOffset || i >= m_CaveXSize - 2 - m_XOffset || j <= 0 - m_YOffset || j >= m_CaveYSize - 2 - m_YOffset)
                    {
                        wall.SetTile(new Vector3Int(i, j, 0), GetTileGroupByTag("BorderBlock").Tiles[4]);
                        continue;
                    }

                    int x = Random.Range(0, 100);
                    if (x < m_MapFillPercent)
                    {
                        wall.SetTile(new Vector3Int(i, j, 0), tileGroup.Tiles[4]);
                    }
                    else continue;
                }
            }
        }

        /// <summary>
        /// Возвращает группу тайлов по тэгу
        /// </summary>
        /// <param name="tag">Тэг поиска</param>
        /// <returns>Группа тайлов</returns>
        private TileGroup GetTileGroupByTag(string tag)
        {
            for (int i = 0; i < m_TileRule.Length; i++)
            {
                if (tag == m_TileRule[i].BlockTag) return m_TileRule[i];
            }

            Debug.LogError("Tag isn't found, returning default");
            return m_TileRule[0];
        }

        /// <summary>
        /// Итерирует стены клеточным автоматом один раз на выбранном тайлмапе.
        /// </summary>
        /// <param name="wall">Целевой Тайлмап</param>
        /// <param name="tileGroup">Группа тайлов/param>
        private void CellAutomataIteration(Tilemap wall, TileGroup tileGroup)
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

                    if (counter > 4) wall.SetTile(new Vector3Int(i, j, 0), tileGroup.Tiles[4]);
                    if (counter < 4) wall.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
        }

        /// <summary>
        /// Алгоритм покраски стен на выбранном тайлмапе по заданным правилам.
        /// </summary>
        /// <param name="wall">Целевой Тайлмап</param>
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
                    WorldShaper.PlaceEditedWallsAltRule(i, j, wall, cg);
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
            if (ore.Tiles.Length == 0) { Debug.LogError("Error: No ore tiles set to " + ore.BlockTag + " tilemap."); return; }

            int rarity = Random.Range(ore.RarityMin, ore.RarityMax + 1);
            rarity += rarity * m_OreMultiplier;

            while (rarity > 0)
            {
                int orecount = ore.VeinSize;
                int xrand = Random.Range(0 - m_XOffset, m_CaveXSize - m_XOffset);
                int yrand = Random.Range(0 - m_YOffset, m_CaveYSize - m_YOffset);
                TileBlockBase tile = wallmap.GetTile<TileBlockBase>(new Vector3Int(xrand, yrand, 0));
                if (tile == null) continue;
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

                        if (tile != null && tile.BlockTag != "BorderBlock" && oremap.GetTile(new Vector3Int(x, y, 0)) == null)
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
        /// Ставит блоки по шуму Перлина
        /// </summary>
        /// <param name="seed">Семя генерации шума, от него будет идти рассчет</param>
        /// <param name="wallmap">Целевой тайлмап</param>
        /// <param name="tileGroup">Группа Тайлов, которые мы хотим поставить</param>
        /// <param name="treshold">Чуствительность к шуму, от 0.1 до 1 включительно. Если значение шума превышает - будет поставлен блок.</param>
        private void GenerateBlocksByPerlinNoise(float seed, Tilemap wallmap, TileGroup tileGroup, float treshold)
        {
            if (treshold > 1) treshold = 1;
            if (treshold < 0.1) treshold = 0.1f;

            seed *= treshold;
            StartCoroutine(PerlinPlacer(m_CaveXSize, m_CaveYSize, m_XOffset, m_YOffset, wallmap, (TileBlockBase)tileGroup.Tiles[4],seed, treshold));
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
        /// Ставит пола на карте, в случайных комнатах
        /// </summary>
        /// <param name="Feature">Фича, которую ставим</param>
        /// <param name="wallTilemap">Тайлмап Стен</param>
        /// <param name="floorTilemap">Тайлмап фич пола</param>
        private void PlaceFeatureInRandomRoom(FeatureSlot Feature, Tilemap wallTilemap, Tilemap floorTilemap)
        {
            if (floorTilemap == null || wallTilemap == null) return;
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
                if (FindEmptyRoomByFlood(rCrd, wallTilemap, floorTilemap, Feature)) continue;
                i--;
                maxIter++;
            }    

        }

        // private List<Vector3Int> m_LatestRoomFloodCoordinates;

        /// <summary>
        /// Алгоритм заполнения заливкой
        /// </summary>
        /// <param name="coordinate">Проверяемая координата</param>
        /// <param name="wallTilemap">Целевой тайлмап стен</param>
        /// <param name="floorTilemap">Целевой тайлмап фич пола</param>
        /// <param name="slot">Фича, которую ставим</param>
        /// <returns></returns>
        private bool FindEmptyRoomByFlood(Vector3Int coordinate, Tilemap wallTilemap, Tilemap floorTilemap, FeatureSlot slot)
        {
            if (wallTilemap.GetTile(coordinate) != null) return false;
            if (floorTilemap.GetTile(coordinate) != null) return false;

            int r = Random.Range(0, 4);

            floorTilemap.SetTile(coordinate, slot.TileRule.Tiles[Random.Range(25, 33)]);

            if (r == 0) TryPlaceFoliage(coordinate);

            //m_LatestRoomFloodCoordinates.Add(stCrd);

            FindEmptyRoomByFlood(new Vector3Int(coordinate.x + 1, coordinate.y), wallTilemap, floorTilemap, slot);
            FindEmptyRoomByFlood(new Vector3Int(coordinate.x, coordinate.y + 1), wallTilemap, floorTilemap, slot);
            FindEmptyRoomByFlood(new Vector3Int(coordinate.x - 1, coordinate.y), wallTilemap, floorTilemap, slot);
            FindEmptyRoomByFlood(new Vector3Int(coordinate.x, coordinate.y - 1), wallTilemap, floorTilemap, slot);

            return true;
        }

        /// <summary>
        /// Генерирует стартовую комнату
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
            if (m_AllowedFoliage.Length == 0) return;
            TileBlockGrowablle tile = m_AllowedFoliage[Random.Range(0, m_AllowedFoliage.Length)];
            if (tile == null) return;
            int chance = Random.Range(0, tile.GrowthChance / 5);
            if (chance == 0) m_WallsTilemap.SetTile(coord, tile);
        }

    }
}
