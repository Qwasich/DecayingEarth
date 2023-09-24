using UnityEngine;
using UnityEngine.Events;

namespace DecayingEarth
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Creature
    {
        [SerializeField] private Rigidbody2D m_Rigidbody;
        [SerializeField] private Collider2D m_PlayerHitbox;
        /// <summary>
        /// В начале сцены или при респавне - сколько данный объект будет неуязвим
        /// </summary>
        [SerializeField] private float m_StartingTimer = 3f;

        public UnityAction<int, int> UpdateHP;

        [Header("Movement Settings")]
        //[SerializeField] private int m_MaxSpeed;
        [SerializeField] private int m_SpeedMultiplier = 1;

        private Vector2 m_MovementVector;
        public Vector2 MovementVector => m_MovementVector;

        private void Start()
        {
            m_Timer = m_StartingTimer;
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(m_MovementVector.x) > 0.1 || Mathf.Abs(m_MovementVector.y) > 0.1) m_Rigidbody.velocity = new Vector2(0, 0);
            m_Rigidbody.velocity = new Vector2(m_MovementVector.x, m_MovementVector.y) * Time.fixedDeltaTime * m_SpeedMultiplier;
        }

        public void GetAxisParameters(float x, float y) => m_MovementVector = new Vector2(x, y);

        protected override void OnKill()
        {
            Singleton_MouseItemHolder.Instance.DropItem();
            Singleton_CraftingEntryPoint.Instance.DisableCraftingUI();
            Singleton_MouseItemHolder.Instance.HideTooltip();
            Singleton_GlobalChestController.Instance.CloseInventory();
        }

        public override int DealDamage(int damage, float critchance, float critmultiplier)
        {
            if (m_IsInvincible || m_Timer > 0) return 0;
            if (m_CurrentHealth <= 0) return 0;
            m_Timer = m_InvincibilityTimer;
            if (damage <= 0) damage = 1;
            if (critchance > 1) critchance = 1;
            if (critchance < 0) critchance = 0;
            if (critmultiplier < 1.5f) critmultiplier = 1.5f;

            damage -= Mathf.FloorToInt((damage / 100) * m_DamageReduction); // Процент поглощения
            damage -= Mathf.CeilToInt(m_PhysicalArmor * m_ArmorMultiplier); // Броня
            if (damage <= 0) damage = 1;
            if (Random.Range(0, 101) <= critchance * 100) damage = Mathf.CeilToInt(damage * critmultiplier);

            m_CurrentHealth -= damage;
            UpdateHP?.Invoke(m_CurrentHealth, m_MaxHealth);
            PlayAudio(m_DamageSound);
            if (m_CurrentHealth <= 0) OnKill();
            return damage;
        }

        public override int HealDamage(int heal, float critchance, float critmultiplier)
        {
            if (m_IsInvincible) return 0;
            if (heal <= 0) heal = 1;
            if (critchance > 1) critchance = 1;
            if (critchance < 0) critchance = 0;
            if (critmultiplier < 1.5f) critmultiplier = 1.5f;

            if (Random.Range(0, 101) <= critchance * 100) heal = Mathf.CeilToInt(heal * critmultiplier);
            m_CurrentHealth += heal;
            if (m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;
            UpdateHP?.Invoke(m_CurrentHealth, m_MaxHealth);
            return heal;
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(KillYourself))]
        public void KillYourself()
        {
            if (!UnityEditor.EditorApplication.isPlaying) return;
            DealDamage(MaxHealth, 0, 1.5f);
        }
#endif
    }
}
