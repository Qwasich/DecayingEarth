using UnityEngine;

namespace ScorgedEarth
{
    public class ItemWobble : MonoBehaviour
    {
        [SerializeField] private float m_WobbleAltitude;
        [SerializeField] private float m_WobblePeriod;

        private float m_num = 0;


        private void Update()
        {
            if (!enabled) return;
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Mathf.Deg2Rad * m_num) * m_WobbleAltitude, transform.position.z);
            m_num += m_WobblePeriod;
            if (m_num >= 360) m_num = 0;
        }

        private void OnBecameInvisible() => enabled = false;

        private void OnBecameVisible() => enabled = true;
    }
}
