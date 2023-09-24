using UnityEngine;
using UnityEngine.UI;

namespace Racist
{
    public class UISettingButton : UISelectableButton, IScriptableObjectProperty
    {
        [SerializeField] private Setting m_Setting;
        [SerializeField] private Text m_TitleText;
        [SerializeField] private Text m_ValueText;
        [SerializeField] private Image m_PreviousButtonImage;
        [SerializeField] private Image m_NextButtonImage;

        protected override void Start()
        {
            base.Start();
            ApplyProperty(m_Setting);
        }

        public void SetPrevioustValueSetting()
        {
            m_Setting?.SetPreviousValue();
            m_Setting?.Apply();
            UpdateUI();
        }

        public void SetNextValueSetting()
        {
            m_Setting?.SetNextValue();
            m_Setting?.Apply();
            UpdateUI();
        }

        private void UpdateUI()
        {
            m_TitleText.text = m_Setting.Title;
            m_ValueText.text = m_Setting.GetStringValue();

            m_PreviousButtonImage.enabled = !m_Setting.isMinValue;
            m_NextButtonImage.enabled = !m_Setting.isMaxValue;
        }

        public void ApplyProperty(ScriptableObject property)
        {
            if (property == null) return;
            if (property is Setting == false) return;

            m_Setting = property as Setting;

            UpdateUI();
        }
    }
}
