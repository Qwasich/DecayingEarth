using UnityEngine;

namespace Racist
{
    [CreateAssetMenu]
    public class GraphicsQualitySetting : Setting
    {
        private int m_CurrentQualityIndex = 0;
        public override bool isMinValue { get => m_CurrentQualityIndex == 0; }
        public override bool isMaxValue { get => m_CurrentQualityIndex == QualitySettings.names.Length - 1; }

        public override void SetPreviousValue()
        {
            if (!isMinValue) m_CurrentQualityIndex--;
        }

        public override void SetNextValue()
        {
            if (!isMaxValue) m_CurrentQualityIndex++;
        }

        public override object GetValue()
        {
            return QualitySettings.names[m_CurrentQualityIndex];
        }

        public override string GetStringValue()
        {
            return QualitySettings.names[m_CurrentQualityIndex];
        }

        public override void Apply()
        {
            QualitySettings.SetQualityLevel(m_CurrentQualityIndex);
            Save();
        }

        public override void Load()
        {
            m_CurrentQualityIndex = PlayerPrefs.GetInt(m_Title, 0);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(m_Title, m_CurrentQualityIndex);
        }
    }
}
