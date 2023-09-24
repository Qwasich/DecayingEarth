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

        [SerializeField] protected BlockType m_BlockType = BlockType.TOP;
        /// <summary>
        /// ��� �����
        /// </summary>
        public BlockType BlockType => m_BlockType;

        [SerializeField] protected string m_Tag;
        /// <summary>
        /// ��� �����
        /// </summary>
        public string Tag => m_Tag;
        /// <summary>
        /// ��������� �����.
        /// </summary>
        [SerializeField] protected int m_Durability;

        protected int m_MaxDurability;
        public int MaxDurability => m_MaxDurability;

        protected int m_RemainingDurability;
        public int RemainingDurability => m_RemainingDurability;

        [SerializeField] protected Vector2Int m_Size = new Vector2Int(1, 1);
        /// <summary>
        /// ������ �����, ��������� ��� �������� ����������� ��������� ����.
        /// </summary>
        public Vector2Int Size => m_Size;

        /// <summary>
        /// ���� �������, ����� ���� ������������.
        /// </summary>
        [SerializeField] protected bool m_IsIndestructible;

        [SerializeField] protected bool m_InvokeRule = true;
        /// <summary>
        /// ���� �������, ����� �������� ����� ������ �� �������� ������.
        /// </summary>
        public bool InvokeRule => m_InvokeRule;

        [SerializeField] protected bool m_IgnoreRigidbody = false;
        /// <summary>
        /// ���� True, ���������� �������� Rigidbody ��� ���������.
        /// </summary>
        public bool IgnoreRigidbody => m_IgnoreRigidbody;

        /// <summary>
        /// �������, ���������� ��� �����������.
        /// </summary>
        [SerializeField] protected ItemBase[] m_Loot;

        [Header("Audio")]
        [SerializeField] private AudioClip m_DamageSound;
        /// <summary>
        /// ����, �������� ��� �����������
        /// </summary>
        public AudioClip DamageSound => m_DamageSound;

        [SerializeField] private AudioClip m_DestroySound;
        /// <summary>
        /// ����, �������� ��� �����������
        /// </summary>
        public AudioClip DestroySound => m_DestroySound;

        [Header("Animation")]
        [SerializeField] protected bool m_IgnoreAnimationSetup = false;
        [SerializeField] protected Sprite[] m_Animation;
        /// <summary>
        /// ������� �������� �����.
        /// </summary>
        public Sprite[] Animation => m_Animation;

        [SerializeField] protected float m_AnimationSpeed = 1f;
        /// <summary>
        /// �������� ��������
        /// </summary>
        public float AnimationSpeed => m_AnimationSpeed;

        [SerializeField] protected float m_AnimationStartTime = 0f;
        /// <summary>
        /// ��������� ����� ��������
        /// </summary>
        public float AnimationStartTime => m_AnimationStartTime;

        [Header("Light")]
        
        [SerializeField] protected bool m_IsALightSource = false;
        /// <summary>
        /// �������� �� ���� ���������� �����
        /// </summary>
        public bool IsALightSource => m_IsALightSource;

        [SerializeField] protected float m_LightRange = 3;
        /// <summary>
        /// ������ �����
        /// </summary>
        public float LightRange => m_LightRange;

        [SerializeField] protected float m_LightIntensity = 10;
        /// <summary>
        /// ������������� �����
        /// </summary>
        public float LightIntensity => m_LightIntensity;

        [SerializeField] protected Color m_LightColor;
        /// <summary>
        /// ���� �������������� ���������
        /// </summary>
        public Color LightColor => m_LightColor;

        [SerializeField][Range(0,180)] protected float m_SpotAngle = 110;
        /// <summary>
        /// ���� ���������
        /// </summary>
        public float SpotAngle => m_SpotAngle;




        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            m_MaxDurability = m_Durability;
            m_RemainingDurability = m_Durability;

            return base.StartUp(position, tilemap, go);
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
            if (m_IgnoreAnimationSetup) return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);

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
        /// ���������� 0 ���� ���� ���������, 1 ���� ����������� ������, 2 ���� ���� ���������� ��� ������� ��������� ����.
        /// </summary>
        /// <param name="damage">��������� ����</param>
        /// <param name="pos">������� ������� ��������</param>
        /// <param name="tilePosition">������� �����</param>
        /// <returns></returns>
        public virtual int DealDamage(int damage,Vector3 pos, Vector3Int tilePosition)
        {
            if (m_IsIndestructible) return 2;
            if (damage * 5 <= m_MaxDurability) return 2;
            
            

            m_RemainingDurability -= damage;
            
            if (m_RemainingDurability <= 0)
            {
                if (m_DestroySound != null) Singleton_PlayerInfo.Instance.Player.PlayAudio(m_DestroySound);
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
            if (m_DamageSound != null) Singleton_PlayerInfo.Instance.Player.PlayAudio(m_DamageSound);
            return 1;
        }

        /// <summary>
        /// ��������������� �������� ����� ���������
        /// </summary>
        public void Recover() => m_RemainingDurability = m_MaxDurability;


        /// <summary>
        /// ������������� �������� ����� �� ������������ ��������, ������ ���� ������ ����. 
        /// ���� �������� ������ ������������� �������� ����� - ������������� ������������.
        /// </summary>
        /// <param name="dur">�������� ��������</param>
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
