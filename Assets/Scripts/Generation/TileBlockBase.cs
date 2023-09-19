using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DecayingEarth
{
    public enum BlockType
    {
        FLOOR,
        SIDE,
        TOP,
        EFFECT
    }

    public class TileBlockBase : Tile
    {
        [Header("Block Parameters")]

        [SerializeField] private BlockType m_BlockType = BlockType.TOP;
        /// <summary>
        /// Тип блока
        /// </summary>
        public BlockType BlockType => m_BlockType;

        [SerializeField] private string m_Tag;
        /// <summary>
        /// Тэг блока
        /// </summary>
        public string Tag => m_Tag;
        /// <summary>
        /// Прочность блока.
        /// </summary>
        [SerializeField] private int m_Durability;

        private int m_MaxDurability;
        public int MaxDurability => m_MaxDurability;

        private int m_RemainingDurability;
        public int RemainingDurability => m_RemainingDurability;

        [SerializeField] private Vector2Int m_Size = new Vector2Int(1, 1);
        /// <summary>
        /// Размер тайла, необходим для проверки возможности поставить блок.
        /// </summary>
        public Vector2Int Size => m_Size;

        /// <summary>
        /// Если истинно, любой урон игнорируется.
        /// </summary>
        [SerializeField] private bool m_IsIndestructible;

        [SerializeField] private bool m_InvokeRule = true;
        /// <summary>
        /// Если истинно, будет изменять тайлы вокруг по правилам блоков.
        /// </summary>
        public bool InvokeRule => m_InvokeRule;

        [SerializeField] private bool m_IgnoreRigidbody = false;
        /// <summary>
        /// Если True, пропускает проверку Rigidbody при установке.
        /// </summary>
        public bool IgnoreRigidbody => m_IgnoreRigidbody;

        /// <summary>
        /// Предмет, выпадающий при уничтожении.
        /// </summary>
        [SerializeField] private ItemBase[] m_Loot;

        [Header("Animation")]
        [SerializeField] private Sprite[] m_Animation;
        /// <summary>
        /// Спрайты анимации тайла.
        /// </summary>
        public Sprite[] Animation => m_Animation;

        [SerializeField] private float m_AnimationSpeed = 1f;
        /// <summary>
        /// Скорость анимации
        /// </summary>
        public float AnimationSpeed => m_AnimationSpeed;

        [SerializeField] private float m_AnimationStartTime = 0f;
        /// <summary>
        /// Стартовое время анимации
        /// </summary>
        public float AnimationStartTime => m_AnimationStartTime;

        [Header("Light")]
        
        [SerializeField] private bool m_IsALightSource = false;
        /// <summary>
        /// Является ли тайл источником света
        /// </summary>
        public bool IsALightSource => m_IsALightSource;

        [SerializeField] private float m_LightRange = 3;
        /// <summary>
        /// Радиус Света
        /// </summary>
        public float LightRange => m_LightRange;

        [SerializeField] private float m_LightIntensity = 10;
        /// <summary>
        /// Интенсивность света
        /// </summary>
        public float LightIntensity => m_LightIntensity;

        [SerializeField] private Color m_LightColor;
        /// <summary>
        /// Цвет прикрепленного освещения
        /// </summary>
        public Color LightColor => m_LightColor;

        [SerializeField][Range(0,180)] private float m_SpotAngle = 110;
        /// <summary>
        /// Угол освещения
        /// </summary>
        public float SpotAngle => m_SpotAngle;




        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            m_MaxDurability = m_Durability;
            m_RemainingDurability = m_Durability;

            return base.StartUp(position, tilemap, go);
            /*
            if (m_IsALightSource)
            {
                GameObject ls = Instantiate(Singleton_PrefabLibrary.Instance.LightSourcePrefab);
                m_Light = ls.GetComponent<Light>();
                ls.transform.position = new Vector3(0.25f, 0.25f);
            }*/
        }



        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (m_Animation != null && m_Animation.Length > 0)
            {
                tileData.sprite = m_Animation[0];
            }
            else base.GetTileData(position, tilemap, ref tileData);
        }
        
        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            if (m_Animation != null && m_Animation.Length > 0)
            {
                tileAnimationData.animatedSprites = m_Animation;
                tileAnimationData.animationSpeed = m_AnimationSpeed;
                tileAnimationData.animationStartTime = m_AnimationStartTime;
                return true;
            }

            return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }

        /// <summary>
        /// Возвращает 0 если тайл уничтожен, 1 если повреждения прошли, 2 если блок неразрушим или слишком маленький урон.
        /// </summary>
        /// <param name="damage">Наносимый урон</param>
        /// <param name="pos">Позиция справна предмета</param>
        /// <returns></returns>
        public virtual int DealDamage(int damage,Vector3 pos)
        {
            if (m_IsIndestructible) return 2;
            if (damage * 5 <= m_MaxDurability) return 2;
            
            

            m_RemainingDurability -= damage;
            
            if (m_RemainingDurability <= 0)
            {
                if (m_Loot == null) return 0;
                for (int i = 0; i < m_Loot.Length; i++)
                {
                    if (m_Loot[i] == null) continue;
                    GameObject m = Instantiate(Singleton_PrefabLibrary.Instance.DummyItemPrefab);
                    m.GetComponent<PhysicalItem>().InitiateItem(m_Loot[i]);
                    m.transform.position = pos + new Vector3(0.25f, 0.25f);
                }
                m_RemainingDurability = m_MaxDurability;
                return 0;
            }
            else return 1;
        }

        /// <summary>
        /// Восстанавливает здоровье тайла полностью
        /// </summary>
        public void Recover() => m_RemainingDurability = m_MaxDurability;


        /// <summary>
        /// Устанавливает здоровье тайла на определенное значение, должно быть больше нуля. 
        /// Если значение больше максимального здоровья тайла - устанавливает максимальное.
        /// </summary>
        /// <param name="dur">Значение здоровья</param>
        public void SetRemainingDurability(int dur)
        {
            if (!m_IsIndestructible || dur < 1) return;
            if (dur <= m_MaxDurability) m_RemainingDurability = dur;
            else m_RemainingDurability = m_MaxDurability;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/Custom Tiles/Block Tile")]
        public static void CreateCustomTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Block Tile", "New Block Tile", "asset", "Save Block Tile","Assets/Sprites");
            if (path == "") return;

            AssetDatabase.CreateAsset(CreateInstance<TileBlockBase>(), path);
        }
#endif

    }
}
