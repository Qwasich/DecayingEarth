using UnityEngine;

namespace DecayingEarth
{
    [RequireComponent(typeof(Player))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_Renderer;
        [SerializeField] private Sprite m_StandingSprite;
        [SerializeField] private float m_AnimationSpeed = 0.3f;
        [SerializeField] private Animator m_Animator;

        private Player m_Player;

        private float m_VerticalAxis;
        private float m_HorizontalAxis;

        private void Awake()
        {
            m_Player = GetComponent<Player>();
            m_Animator.speed = m_AnimationSpeed;
        }

        private void Update()
        {
            if (!Singleton_SessionData.Instance.IsInventoryHidden)
            {
                m_Player.GetAxisParameters(0, 0);
                return;
            }
            UpdateMoveAxis();
            UpdatePlayer();
            UpdateAnimation();


        }

        private void UpdateMoveAxis()
        {
            m_VerticalAxis = Input.GetAxis("Vertical");
            m_HorizontalAxis = Input.GetAxis("Horizontal");
        }

        private void UpdateAnimation()
        {
            if (m_VerticalAxis == 0 && m_HorizontalAxis == 0 && m_Renderer.sprite != m_StandingSprite)
            {
                m_Animator.StartPlayback();
                m_Renderer.sprite = m_StandingSprite;
                return;
            }
            if (m_HorizontalAxis > 0) m_Renderer.flipX = false;
            if (m_HorizontalAxis < 0) m_Renderer.flipX = true;

            if (m_VerticalAxis != 0 || m_HorizontalAxis != 0) m_Animator.StopPlayback();
        }

        private void UpdatePlayer()
        {
            if (m_Player == null) return;
            m_Player.GetAxisParameters(m_HorizontalAxis, m_VerticalAxis);
        }
    }
}
