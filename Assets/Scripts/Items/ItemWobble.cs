using UnityEngine;

namespace DecayingEarth
{
    public class ItemWobble : MonoBehaviour
    {
        [SerializeField] private float m_WobbleAltitude;
        [SerializeField] private float m_WobblePeriod;

        private float m_num = 0;


        private void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Mathf.Deg2Rad * m_num) * m_WobbleAltitude, transform.position.z);
            m_num += m_WobblePeriod * Time.deltaTime;
            if (m_num >= 360) m_num = 0;
        }
    }
}
