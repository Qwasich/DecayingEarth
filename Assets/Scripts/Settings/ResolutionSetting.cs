using UnityEngine;

namespace Racist
{
    [CreateAssetMenu]
    public class ResolutionSetting : Setting
    {
        [SerializeField]
        private Vector2Int[] m_Resolutions = new Vector2Int[]
        {
            new Vector2Int(800, 600),
            new Vector2Int(1280, 720),
            new Vector2Int(1600, 900),
            new Vector2Int(1920, 1080)
        };

        private int m_ResolutionIndex;

        public override bool isMinValue { get => m_ResolutionIndex == 0; }
        public override bool isMaxValue { get => m_ResolutionIndex == m_Resolutions.Length - 1; }

        public override void SetPreviousValue()
        {
            if (!isMinValue) m_ResolutionIndex--;
            
        }

        public override void SetNextValue()
        {
            if (!isMaxValue) m_ResolutionIndex++;
        }

        public override object GetValue()
        {
            return m_Resolutions[m_ResolutionIndex];
        }

        public override string GetStringValue()
        {
            return m_Resolutions[m_ResolutionIndex].x + "x" + m_Resolutions[m_ResolutionIndex].y;
        }

        public override void Apply()
        {
            Screen.SetResolution(m_Resolutions[m_ResolutionIndex].x, m_Resolutions[m_ResolutionIndex].y,Screen.fullScreenMode);

            Save();
        }

        public override void Load()
        {
            m_ResolutionIndex = PlayerPrefs.GetInt(m_Title, 0);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(m_Title, m_ResolutionIndex);
        }

    }
}
