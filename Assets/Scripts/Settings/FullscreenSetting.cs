using UnityEngine;

namespace Racist
{
    [CreateAssetMenu]
    public class FullscreenSetting : Setting
    {

        private int m_FullscreenIndex;

        public override bool isMinValue { get => m_FullscreenIndex == 1; }
        public override bool isMaxValue { get => m_FullscreenIndex == 3; }

        public override void SetPreviousValue()
        {
            if (!isMinValue) m_FullscreenIndex--;

        }

        public override void SetNextValue()
        {
            if (!isMaxValue) m_FullscreenIndex++;
        }

        public override object GetValue()
        {
            return (FullScreenMode)m_FullscreenIndex;
        }

        public override string GetStringValue()
        {
            FullScreenMode mode = (FullScreenMode)m_FullscreenIndex;
            return mode.ToString();
        }

        public override void Apply()
        {
            Screen.fullScreenMode = (FullScreenMode)m_FullscreenIndex;

            Save();
        }

        public override void Load()
        {
            m_FullscreenIndex = PlayerPrefs.GetInt(m_Title, 3);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(m_Title, m_FullscreenIndex);
        }

    }
}
