
using Unity.VisualScripting;
using UnityEngine;

namespace DecayingEarth
{
    public class WeaponCollisionHandler : MonoBehaviour
    {
        private PolygonCollider2D m_WeaponCollider;
        [SerializeField] private SpriteRenderer m_SpriteObject;

        private ItemBase m_Item;
        
        public void Use(ItemBase weapon)
        {
            m_Item = weapon;
            if (m_Item == null || m_Item.Icon == null) return;
            if (m_WeaponCollider != null) EndUse();
            m_WeaponCollider = gameObject.AddComponent<PolygonCollider2D>();
            m_WeaponCollider.isTrigger = true;
        }

        public void EndUse()
        {
            if (m_WeaponCollider == null) return;
            Destroy(m_WeaponCollider);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "PickupHitbox") return;
            Creature creat = collision.gameObject.transform.root.GetComponent<Creature>();
            if (creat == null) return;
            var item = m_Item;
            if (item is ItemWeapon || item is ItemTool)
            {
                if (item is ItemWeapon && !(item as ItemWeapon).DoesWeaponSpriteDealsDamage) return;
                else if (item is ItemTool && !(item as ItemTool).DoesWeaponSpriteDealsDamage) return;

                creat.DealDamage((item as ItemWeapon).DealtDamage, 0, 1.5f);

            }

        }

    }
}
