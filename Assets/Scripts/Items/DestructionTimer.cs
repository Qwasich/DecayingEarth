using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    public class DestructionTimer : MonoBehaviour
    {
        [SerializeField] private int m_TimerInSeconds;

        private float m_Timer;
        public float RemaninigTime => m_Timer;

        private void Awake()
        {
            m_Timer = m_TimerInSeconds;
        }

        private void Update()
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0) Destroy(gameObject);
        }
    }
}
