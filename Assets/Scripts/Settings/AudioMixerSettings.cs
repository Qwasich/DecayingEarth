using UnityEngine;
using UnityEngine.Audio;

namespace Racist
{
    [CreateAssetMenu]
    public class AudioMixerSettings : Setting
    {
        [SerializeField] private AudioMixer m_Mixer;
        [SerializeField] private string m_ParameterName;

        
        [SerializeField] private float m_MinRealValue;
        [SerializeField] private float m_MaxRealValue;

        [SerializeField] private float m_VirtualStep;
        [SerializeField] private float m_MinVirtualValue;
        [SerializeField] private float m_MaxVirtualValue;

        float m_CurrentValue = 0;

        public override bool isMinValue { get => m_CurrentValue == m_MinRealValue; }
        public override bool isMaxValue { get => m_CurrentValue == m_MaxRealValue; }

        public override void SetPreviousValue()
        {
            AddValue(-Mathf.Abs(m_MaxRealValue - m_MinRealValue) / m_VirtualStep);
        }

        public override void SetNextValue()
        {
            AddValue(Mathf.Abs(m_MaxRealValue - m_MinRealValue) / m_VirtualStep);
        }

        public override string GetStringValue()
        {
            return Mathf.Lerp(m_MinVirtualValue, m_MaxVirtualValue,(m_CurrentValue - m_MinRealValue)/ (m_MaxRealValue - m_MinRealValue)).ToString();
        }

        public override object GetValue()
        {
            return m_CurrentValue;
        }

        public override void Apply()
        {
            m_Mixer.SetFloat(m_ParameterName, m_CurrentValue);

            Save();
        }

        private void AddValue(float value)
        {
            m_CurrentValue += value;

            m_CurrentValue = Mathf.Clamp(m_CurrentValue, m_MinRealValue, m_MaxRealValue);
        }

        public override void Load()
        {
            m_CurrentValue = PlayerPrefs.GetFloat(m_Title, 0);
        }

        private void Save()
        {
            PlayerPrefs.SetFloat(m_Title, m_CurrentValue);
        }

    }
}
