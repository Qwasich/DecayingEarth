using System.Collections;
using UnityEngine;

namespace DecayingEarth
{
    public class VineCreeperEnemy : Creature
    {
        [SerializeField] private Rigidbody2D m_Rigidbody;

        [SerializeField] private SpriteRenderer m_Renderer;

        [SerializeField] private RuntimeAnimatorController m_WakeUpAnim;
        [SerializeField] private RuntimeAnimatorController m_RunAnim;
        [SerializeField] private RuntimeAnimatorController m_DeathAnim;
        [SerializeField] private Animator m_TargetAnimator;

        [SerializeField] private PlayerItemAnimationController m_MonsterController;
        [SerializeField] private WeaponCollisionHandler m_WeaponCollisionHandler;

        [SerializeField] private float AggroDistance = 1;
        [SerializeField] private float SwingAggroDistance = 1;

        [SerializeField] private float m_RunSpeed = 3;

        [SerializeField] private ItemBase m_UsedItem;

        [SerializeField] private float m_UseTimer = 3;

        private Vector2 m_MovementVector;

        private bool m_PlayerSpotted = false;

        private float m_SwingTimer = 0;

        private bool colCheck = true;

        private void Start()
        {
            StartCoroutine(AI_Wait());
        }

        protected override void Update()
        {
            base.Update();
            if (m_SwingTimer > 0) m_SwingTimer -= Time.deltaTime;
            if (m_PlayerSpotted)
            {
                if (m_SwingTimer <= 0 && colCheck && CheckPlayerNearby(SwingAggroDistance))
                {
                    m_MonsterController.PlayAnimation(m_UsedItem.UseTimer, m_UsedItem.HoldType, m_UsedItem.Icon, m_UsedItem.SwingAngle, Singleton_PlayerInfo.Instance.Player.transform.position);
                    m_WeaponCollisionHandler.Use(m_UsedItem);
                    colCheck = false;
                    m_SwingTimer = m_UseTimer;

                }
                if (colCheck == false && (m_SwingTimer < m_UseTimer - m_UsedItem.UseTimer || m_SwingTimer <= 0))
                {
                    colCheck = true;
                    m_WeaponCollisionHandler.EndUse();
                }

            }
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(m_MovementVector.x) > 0.1 || Mathf.Abs(m_MovementVector.y) > 0.1) m_Rigidbody.velocity = new Vector2(0, 0);
            if (m_PlayerSpotted && !CheckPlayerNearby(SwingAggroDistance))
            {
                m_MovementVector = (Singleton_PlayerInfo.Instance.Player.transform.position - transform.position).normalized;

                if (m_MovementVector.x > 0) m_Renderer.flipX = false;
                if (m_MovementVector.x < 0) m_Renderer.flipX = true;

                m_Rigidbody.velocity = new Vector2(m_MovementVector.x, m_MovementVector.y) * Time.fixedDeltaTime * m_RunSpeed;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Creature cr = collision.gameObject.GetComponent<Creature>();
            if (cr != null) return;
        }

        public IEnumerator AI_Wait()
        {
            bool check = true;
            while(check)
            {
                yield return new WaitForSeconds(1);
                bool p = CheckPlayerNearby(AggroDistance);
                bool d = CheckIfDamaged();

                if (p || d)
                {
                    m_PlayerSpotted = true;
                    WakeUp();
                    m_SwingTimer = m_UseTimer;
                    StartCoroutine(PlayAnim(m_RunAnim, 0.5f));
                    break;
                }
            }

        }

        public IEnumerator PlayAnim(RuntimeAnimatorController anim, float delay)
        {
            yield return new WaitForSeconds(delay);
            m_TargetAnimator.runtimeAnimatorController = anim;
        }


        private bool CheckPlayerNearby(float pos)
        {
            float d = Vector2.Distance(Singleton_PlayerInfo.Instance.Player.gameObject.transform.position, transform.position);
            if (d <= pos) return true;
            return false;
        }

        private bool CheckIfDamaged()
        {
            if (m_CurrentHealth < m_MaxHealth) return true;
            return false;
        }

        private void WakeUp()
        {
            m_TargetAnimator.enabled = true;
            m_TargetAnimator.runtimeAnimatorController = m_WakeUpAnim;
        }


    }
}
