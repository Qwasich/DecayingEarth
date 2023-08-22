using UnityEngine;

namespace ScorgedEarth
{
    public class PlayerPickUp : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D m_PickupCollider;
        [SerializeField] private InvEntryPoint m_PlayerInventory;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PhysicalItem item = collision.gameObject.GetComponent<PhysicalItem>();
            if (item == null) return;

            m_PlayerInventory.PickUpItem(item);

        }

    }
}
