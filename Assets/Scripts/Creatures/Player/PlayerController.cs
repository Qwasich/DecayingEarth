using UnityEngine;

namespace ScorgedEarth
{
    [RequireComponent(typeof(Player))]
    public class PlayerController : MonoBehaviour
    {
        private Player m_Player;

        private float m_VerticalAxis;
        private float m_HorizontalAxis;

        private void Awake()
        {
            m_Player = GetComponent<Player>();
        }

        private void Update()
        {
            UpdateAxis();
            UpdatePlayer();
        }

        private void UpdateAxis()
        {
            m_VerticalAxis = Input.GetAxis("Vertical");
            m_HorizontalAxis = Input.GetAxis("Horizontal");
        }

        private void UpdatePlayer()
        {
            if (m_Player == null) return;
            m_Player.GetAxisParameters(m_HorizontalAxis, m_VerticalAxis);
        }
    }
}
