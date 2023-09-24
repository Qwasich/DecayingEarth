using UnityEngine;

namespace DecayingEarth
{
    public class PlayerPickUp : MonoBehaviour
    {
        [SerializeField] private InvEntryPoint m_PlayerInventory;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Singleton_PlayerInfo.Instance.Player.CurrentHealth <= 0) return;
            PhysicalItem item = collision.gameObject.GetComponent<PhysicalItem>();
            if (item == null) return;

            m_PlayerInventory.PickUpItem(item);

        }

    }
}
