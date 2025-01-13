using UnityEngine;
using UnityEngine.Tilemaps;

namespace DecayingEarth
{
    public class Projectile : MonoBehaviour
    {
        private ItemProjectile m_DropItem;
        /// <summary>
        /// Снаряд
        /// </summary>
        public ItemBase DropItem => m_DropItem;

        private Creature m_Parent;
        /// <summary>
        /// Родитель, создавший этот объект
        /// </summary>
        public Creature Parent => m_Parent;

        private int m_AddedDamage = 0;
        /// <summary>
        /// Урон от оружия, добавляемый к урону снаряда
        /// </summary>
        public int AddedDamage => m_AddedDamage;


        

        private float m_Timer = 0;

        private void Update()
        {
            float stepLength = Time.deltaTime * m_DropItem.Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (rayHit)
            {
                Creature creat = rayHit.collider.transform.root.GetComponent<Creature>();
                TileBlockBase tile = (TileBlockBase) Singleton_GridLibrary.Instance.WallsTilemap.GetTile(Singleton_GridLibrary.Instance.WallsTilemap.WorldToCell(transform.position));

                if (creat != null && creat != m_Parent)
                {
                    creat.DealDamage(m_DropItem.DealtDamage, 0, 1.5f);
                    OnLifetimeEnd();
                }
                if (tile != null && tile.colliderType != Tile.ColliderType.None) OnLifetimeEnd();
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_DropItem.MaxLifetime)
            {
                OnLifetimeEnd();
            }

            transform.position += new Vector3(step.x, step.y, 0);

        }

        private void OnLifetimeEnd()
        {
            /*
            if (m_DropItem.IsDroppableOnCollision)
            {
                GameObject go = Instantiate(Singleton_PrefabLibrary.Instance.DummyItemPrefab, transform);
                PhysicalItem pi = go.GetComponent<PhysicalItem>();
                pi.InitiateItem(m_DropItem, 1, false);
            }*/
            Destroy(gameObject);

        }


        public void Initialize(ItemProjectile projectile, int addedDamage)
        {
            if (projectile == null) return;
            m_DropItem = projectile;
            AddDamage(addedDamage);

            SpriteRenderer spr = GetComponentInChildren<SpriteRenderer>();
            if (spr == null) return;

            spr.sprite = m_DropItem.Icon;
            
        }


        /// <summary>
        /// Назначить родителя снаряда
        /// </summary>
        /// <param name="parent">Родитель</param>
        public void SetParentShooter(Creature parent)
        {
            m_Parent = parent;
        }

        /// <summary>
        /// Увеличивает урон снаряда. Применяется, чтобы добавить урон оружия к урону снаряда
        /// </summary>
        /// <param name="damage"></param>
        private void AddDamage(int damage)
        {
            if (damage <= 0) return;

            m_AddedDamage += damage;
        }



    }
}
