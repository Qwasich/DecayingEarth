using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DecayingEarth
{
    public abstract class Creature : MonoBehaviour
    {
        
        [Header("Creature Parameters")]
        [SerializeField] protected string m_Nickname;
        /// <summary>
        /// ��� ��������
        /// </summary>
        public string Name => m_Nickname;

        [SerializeField] protected int m_MaxHealth;
        protected int m_CurrentHealth;

        public int MaxHealth => m_MaxHealth;
        public int CurrentHealth => m_CurrentHealth;
        

        [SerializeField] protected bool m_IsInvincible;
        /// <summary>
        /// ���� �������, ���������� ����� ����.
        /// </summary>
        public bool IsInvincible => m_IsInvincible;

        [SerializeField] protected int m_PhysicalArmor;
        /// <summary>
        /// ���������� �����, �������� ���� ��: ���� = ���� - ����� * �����������. ����������� ����� ��� ����������.
        /// </summary>
        public int PhysicalArmor => m_PhysicalArmor;

        [SerializeField][Range(0,1)] protected float m_ArmorMultiplier = 0.5f;
        /// <summary>
        /// ����������� �����, �������� ���������� �� ����, ����� �������� �����.
        /// </summary>
        public float ArmorMultiplier => m_ArmorMultiplier;

        [SerializeField][Range(0, 100)] protected int m_DamageReduction = 0;
        /// <summary>
        /// ������� �����, ������������ ���������. ������� ����������� ����������, ����� - �����. ����������� ���� ��� ����������.
        /// </summary>
        public int DamageReduction => m_DamageReduction;

        [SerializeField] protected float m_InvincibilityTimer = 0.05f;
        /// <summary>
        /// ������� ������ �������� ���������� � ������������, ����� ��������� �����
        /// </summary>
        public float InincibilityTimer => m_InvincibilityTimer;

        protected float m_Timer;
        [Header("Audio")]
        [SerializeField] protected AudioSource m_AudioSource;
        /// <summary>
        /// �������� �����, ������������� � ��������.
        /// </summary>
        public AudioSource AudioSource => m_AudioSource;

        [SerializeField] protected AudioClip m_DamageSound;
        /// <summary>
        /// ����, �������� ��� �����������
        /// </summary>
        public AudioClip DamageSound => m_DamageSound;

        [SerializeField] protected AudioClip m_DestroySound;
        /// <summary>
        /// ����, �������� ��� �����������
        /// </summary>
        public AudioClip DestroySound => m_DestroySound;





        #region Unity Functions

        protected virtual void Awake()
        {
            m_CurrentHealth = m_MaxHealth;
        }
        #endregion

        protected virtual void Update()
        {
            if (m_Timer > 0) m_Timer -= Time.deltaTime;
        }

        #region Puclic Functions
        /// <summary>
        /// ������� ����. ���� ��������� �����, ������ ������� �������, ���� ��� ��� ������ �������� - ������ 0;
        /// </summary>
        /// <param name="damage">���� ������ ���� ������ ��� ����� �������. ���� ������ - ����������� �� 1.</param>
        /// <param name="critchance">���� �����, � ��������� �� 0 �� 1 ������������.
        /// ������� ��� ������� �������� ����� ������������� ���������� �� ���������������� �������. </param>
        /// <param name="critmultiplier">��������� ������������ �����, 1.5 - �����������. �������� �� ���������.</param>
        /// <returns></returns>
        public virtual int DealDamage(int damage, float critchance, float critmultiplier)
        {
            if (m_IsInvincible || m_Timer > 0) return 0;
            if (m_CurrentHealth <= 0) return 0;
            m_Timer = m_InvincibilityTimer;
            if (damage <= 0) damage = 1;
            if (critchance > 1) critchance = 1;
            if (critchance < 0) critchance = 0;
            if (critmultiplier < 1.5f) critmultiplier = 1.5f;

            damage -= Mathf.FloorToInt((damage / 100) * m_DamageReduction); // ������� ����������
            damage -= Mathf.CeilToInt(m_PhysicalArmor * m_ArmorMultiplier); // �����
            if (damage <= 0) damage = 1;
            if (Random.Range(0, 101) <= critchance * 100) damage = Mathf.CeilToInt(damage * critmultiplier);

            m_CurrentHealth -= damage;
            PlayAudio(m_DamageSound);
            if (m_CurrentHealth <= 0) OnKill();
            return damage;
        }

        public virtual int HealDamage(int heal, float critchance, float critmultiplier)
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

        public void PlayAudio(AudioClip audio)
        {
            if (audio == null) return;
            float pitch = 1f;

            pitch += Random.Range(-0.5f, 0.5f);

            m_AudioSource.pitch = pitch;
            m_AudioSource.clip = audio;
            m_AudioSource.Play();
        }

        protected virtual void OnKill()
        {
            PlayAudio(m_DestroySound);
            Destroy(gameObject);
        }

        #endregion
    }
}
