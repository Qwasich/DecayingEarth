using System.Collections;
using UnityEngine;

namespace DecayingEarth
{
    public class PlayerItemAnimationController : MonoBehaviour
    {
        [SerializeField] private GameObject m_ToolModel;
        [SerializeField] private SpriteRenderer m_Sprite;
        [SerializeField] private float m_MaxOffsetAngle = 110;
        private float m_LatestAngle = 0;

        private void Start()
        {
            m_LatestAngle = m_MaxOffsetAngle;
        }


        public void PlayAnimation(int playTime, HoldType hold, Sprite sprite)
        {
            switch(hold)
            {
                case HoldType.Swing:
                    {
                        StartCoroutine(Swing(playTime));
                        return;
                    }
                case HoldType.Pierce:
                    {
                        m_ToolModel.transform.rotation = Quaternion.Euler(0, 0, 0);
                        StartCoroutine(Pierce(playTime));
                        return;
                    }
                case HoldType.Static:
                    {
                        m_ToolModel.transform.rotation = Quaternion.Euler(0, 0, 0);
                        StartCoroutine(Hold(playTime));
                        return;
                    }
                case HoldType.Empty:
                    {
                        return;
                    }
            }
        }

        IEnumerator Swing(int playTime)
        {
            yield return null;
        }

        IEnumerator Pierce(int playTime)
        {
            yield return null;
        }

        IEnumerator Hold(int playTime)
        {
            yield return null;
        }
    }
}
