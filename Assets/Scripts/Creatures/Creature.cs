using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    public abstract class Creature : MonoBehaviour
    {
        
        [Header("Creature Parameters")]
        [SerializeField] private string m_Nickname;
        /// <summary>
        /// Имя Существа
        /// </summary>
        public string Name => m_Nickname;

        [SerializeField] private int m_MaxHealth;
        private int m_CurrentHealth;

        public int MaxHealth => m_MaxHealth;
        public int CurrentHealth => m_CurrentHealth;
        

        [SerializeField] private bool m_IsInvincible;
        /// <summary>
        /// Если истинно, игнорирует любой урон.
        /// </summary>
        public bool IsInvincible => m_IsInvincible;

        [SerializeField] private int m_PhysicalArmor;
        /// <summary>
        /// Физическая броня, вычитает урон по: Урон = Урон - Броня * модификатор. Округляется вверх при вычислении.
        /// </summary>
        public int PhysicalArmor => m_PhysicalArmor;

        [SerializeField][Range(0,1)] private float m_ArmorMultiplier = 0.5f;
        /// <summary>
        /// Модификатор блока, напрямую умножается на блок, чтобы получить число.
        /// </summary>
        public float ArmorMultiplier => m_ArmorMultiplier;

        [SerializeField][Range(0, 100)] private int m_DamageReduction = 0;
        /// <summary>
        /// Процент урона, поглощаемого существом. Сначала применяется поглощение, потом - броня. Округляется вниз при вычислении.
        /// </summary>
        public int DamageReduction => m_DamageReduction;

        #region Unity Functions

        protected virtual void Awake()
        {
            m_CurrentHealth = m_MaxHealth;
        }
        #endregion

        #region Puclic Functions
        /// <summary>
        /// Нанести урон. Если параметры верны, вернет минимум единицу, если нет или объект неуязвим - вернет 0;
        /// </summary>
        /// <param name="damage">Урон должен быть больше или равен единице. Если меньше - округляется до 1.</param>
        /// <param name="critchance">Шанс крита, в диапазоне от 0 до 1 включительно.
        /// Большее или меньшее значение будет принудительно округлятся до соответствующего предела. </param>
        /// <param name="critmultiplier">Множитель критического урона, 1.5 - минимальный. Максимум не ограничен.</param>
        /// <returns></returns>
        public int DealDamage(int damage, float critchance, float critmultiplier)
        {
            if (m_IsInvincible) return 0;
            if (damage <= 0) damage = 1;
            if (critchance > 1) critchance = 1;
            if (critchance < 0) critchance = 0;
            if (critmultiplier < 1.5f) critmultiplier = 1.5f;

            damage -= Mathf.FloorToInt((damage / 100) * m_DamageReduction); // Процент поглощения
            damage -= Mathf.CeilToInt(m_PhysicalArmor * m_ArmorMultiplier); // Броня
            if (damage <= 0) damage = 1;
            if (Random.Range(0, 101) <= critchance * 100) damage = Mathf.CeilToInt(damage * critmultiplier);

            m_CurrentHealth -= damage;
            if (m_CurrentHealth <= 0) OnKill();
            return damage;
        }

        public int HealDamage(int heal, float critchance, float critmultiplier)
        {
            if (m_IsInvincible) return 0;
            if (heal <= 0) heal = 1;
            if (critchance > 1) critchance = 1;
            if (critchance < 0) critchance = 0;
            if (critmultiplier < 1.5f) critmultiplier = 1.5f;

            if (Random.Range(0, 101) <= critchance * 100) heal = Mathf.CeilToInt(heal * critmultiplier);
            m_CurrentHealth += heal;
            if (m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;

            return heal;
        }

        public void OnKill()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}
