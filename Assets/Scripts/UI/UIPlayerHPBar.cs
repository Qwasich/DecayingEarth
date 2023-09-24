using UnityEngine;
using UnityEngine.UI;

namespace DecayingEarth
{
    public class UIPlayerHPBar : MonoBehaviour
    {
        [SerializeField] private Player m_TargetPlayer;
        [SerializeField] private Image m_HPBar;

        private void Start()
        {
            m_TargetPlayer.UpdateHP += OnHpChange;
        }

        private void OnDestroy()
        {
            m_TargetPlayer.UpdateHP -= OnHpChange;
        }

        private void OnHpChange(int current, int max)
        {
            float percent = (float)current / max;
            m_HPBar.fillAmount = percent;
        }

    }
}
