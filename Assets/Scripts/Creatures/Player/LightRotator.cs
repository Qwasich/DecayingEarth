using UnityEngine;

namespace DecayingEarth
{
    public class LightRotator : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D m_player;

        private void Update()
        {
            if (m_player == null) return;
            if (m_player.velocity == Vector2.zero) return;

            Quaternion rot = Quaternion.LookRotation(Vector3.forward, m_player.velocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot,  1000 * Time.deltaTime);
            
        }
    }
}
