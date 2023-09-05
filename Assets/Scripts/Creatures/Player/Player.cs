using UnityEngine;

namespace ScourgedEarth
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Creature
    {
        [SerializeField] private Rigidbody2D m_Rigidbody;
        [SerializeField] private Collider2D m_PlayerHitbox;

        [Header("Movement Settings")]
        //[SerializeField] private int m_MaxSpeed;
        [SerializeField] private int m_SpeedMultiplier = 1;

        private Vector2 m_MovementVector;
        public Vector2 MovementVector => m_MovementVector;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(m_MovementVector.x) > 0.1 || Mathf.Abs(m_MovementVector.y) > 0.1) m_Rigidbody.velocity = new Vector2(0, 0);
            m_Rigidbody.velocity = new Vector2(m_MovementVector.x, m_MovementVector.y) * Time.fixedDeltaTime * m_SpeedMultiplier;
        }

        public void GetAxisParameters(float x, float y) => m_MovementVector = new Vector2(x, y);
    }
}
