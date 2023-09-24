using UnityEngine;
using Utility;

namespace DecayingEarth
{
    public class Singleton_PlayerInfo : MonoSingleton<Singleton_PlayerInfo>
    {
        [SerializeField] private Player m_Player;
        /// <summary>
        /// Текущий активный игрок
        /// </summary>
        public Player Player => m_Player;

        [SerializeField] private Vector3Int m_RespawnLocation = new Vector3Int(0, 0, 0);
        /// <summary>
        /// Локация, где игрок появится после смерти
        /// </summary>
        public Vector3Int RespawnLocation => m_RespawnLocation;

        [SerializeField] private PlayerItemUser m_User;
        /// <summary>
        /// Исползовальщик предметов, да
        /// </summary>
        public PlayerItemUser User => m_User;

        [SerializeField] private InvUIHandler m_UIinv;

        [SerializeField] private GameObject m_UIDeathScreen;

        [SerializeField] private float m_RespawnTimer = 5f;

        private bool m_IsDead = false;

        private float m_Timer;

        protected override void Awake()
        {
            m_Player.gameObject.SetActive(true); ;
            base.Awake();
        }

        private void Update()
        {
            if (m_Timer > 0) m_Timer -= Time.deltaTime;

            if (m_Player != null && !m_IsDead)
            {
                if (m_Player.CurrentHealth > 0) return;

                if (m_UIDeathScreen != null) m_UIDeathScreen.SetActive(true);

                m_UIinv.ChangeInventory();

                m_Timer = m_RespawnTimer;
                m_IsDead = true;
            }

            if (m_IsDead && m_Timer <= 0 && m_Player != null && m_Player.CurrentHealth <= 0)
            {
                if (m_UIDeathScreen != null) m_UIDeathScreen.SetActive(false);
                m_Player.gameObject.transform.position = m_RespawnLocation;
                m_Player.HealDamage(m_Player.MaxHealth, 0, 1.5f);

                m_IsDead = false;

            }
        }
    }
}
