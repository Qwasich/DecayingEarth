using System.Collections;
using UnityEngine;

namespace DecayingEarth
{
    public class PlayerItemAnimationController : MonoBehaviour
    {
        [SerializeField] private GameObject m_ToolModel;
        [SerializeField] private SpriteRenderer m_Sprite;
        private float m_LatestAngle = 0;

        private bool m_InverseSwing = false;


        private bool m_IsCoroutineRunning = false;
        /// <summary>
        /// Используется для проверки, запущена ли анимация или нет.
        /// </summary>
        public bool IsCoroutineRunning => m_IsCoroutineRunning;

        /// <summary>
        /// Играет анимацию спрайтом в зависимости от типа 
        /// </summary>
        /// <param name="playTime">Время, за которое выполняется анимация</param>
        /// <param name="hold">Тип удержания</param>
        /// <param name="sprite">Спрайт, использующийся для анимации</param>
        public void PlayAnimation(float playTime, HoldType hold, Sprite sprite, float offsetAngle)
        {
            if (!Singleton_SessionData.Instance.IsInventoryHidden) return;
            m_LatestAngle = offsetAngle;
            //if (m_IsCoroutineRunning) return;
            StopAllCoroutines();
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mouse - m_ToolModel.transform.position;

            float angle = Vector2.SignedAngle(Vector2.right, dir);
            m_ToolModel.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            switch (hold)
            {
                case HoldType.Swing:
                    {
                        if (m_Sprite != null && sprite != null) m_Sprite.sprite = sprite;
                        m_ToolModel.transform.localPosition = Vector3.zero;
                        StartCoroutine(Swing(playTime, angle, offsetAngle));
                        break;
                    }
                case HoldType.Pierce:
                    {
                        if (m_Sprite != null && sprite != null) m_Sprite.sprite = sprite;
                        m_ToolModel.transform.localPosition = Vector3.zero;
                        m_ToolModel.transform.localScale = new Vector3(1, 1, 1);
                        StartCoroutine(Pierce(playTime));
                        break;
                    }
                case HoldType.Hold:
                    {
                        if (m_Sprite != null && sprite != null) m_Sprite.sprite = sprite;
                        m_ToolModel.transform.localPosition = Vector3.zero;
                        m_ToolModel.transform.localScale = new Vector3(1, 1, 1);
                        StartCoroutine(Hold(playTime));
                        break;
                    }
                case HoldType.Empty:
                    {
                        break;
                    }
            }
        }

        private void ResetSprite()
        {
            //if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) return;
            m_Sprite.sprite = Singleton_PrefabLibrary.Instance.EmptySprite;
        }

        IEnumerator Swing(float playTime, float angle, float offsetAngle)
        {
            m_IsCoroutineRunning = true;
            float fromAngle;
            float toAngle;

            if (!m_InverseSwing)
            {
                fromAngle = angle + 90 - offsetAngle + 180;
                toAngle = angle + 90 + offsetAngle + 180;
                m_ToolModel.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                fromAngle = angle + 90 + offsetAngle + 180;
                toAngle = angle + 90 - offsetAngle + 180;
                m_ToolModel.transform.localScale = new Vector3(-1, 1, 1);
            }


            float t = 0f;
            int it = 0;

            while (t < 1f && it < 10000)
            {
                float prefix = (Time.deltaTime + Mathf.Sin(t * 180 * Mathf.Deg2Rad) * 5) * Time.deltaTime;

                m_LatestAngle = Mathf.Lerp(fromAngle, toAngle, t);

                t += (prefix) / playTime;

                m_ToolModel.transform.rotation = Quaternion.Euler(0, 0, m_LatestAngle);
                it++;
                
                yield return null;
            }
            m_InverseSwing = !m_InverseSwing;
            ResetSprite();
            m_IsCoroutineRunning = false;
        }

        IEnumerator Pierce(float playTime)
        {
            m_IsCoroutineRunning = true;

            float length = Mathf.Sqrt(Mathf.Pow(m_Sprite.sprite.textureRect.width,2) + Mathf.Pow(m_Sprite.sprite.textureRect.height,2)) / m_Sprite.sprite.pixelsPerUnit;

            float travelDistance = length * m_Sprite.sprite.pixelsPerUnit * 0.75f;

            float t = 0f;
            int it = 0;

            Vector3 mod = m_ToolModel.transform.up;
            Debug.Log(mod);
            Vector3 startPos = m_ToolModel.transform.localPosition - new Vector3(mod.x * length / 2, mod.y * length / 2, mod.z * length / 2);


            while (t < 1f && it < 10000)
            {
                float prefix = (Time.deltaTime + Mathf.Sin(t * 180 * Mathf.Deg2Rad) * 5) * Time.deltaTime;

                t += (prefix) / playTime;

                m_ToolModel.transform.localPosition = new Vector3(startPos.x + (prefix * mod.x * travelDistance), startPos.y + (prefix * mod.y * travelDistance), startPos.z + (prefix * mod.z * travelDistance));

                it++;

                yield return null;
            }

            ResetSprite();
            m_IsCoroutineRunning = false;
        }

        IEnumerator Hold(float playTime)
        {
            m_IsCoroutineRunning = true;

            float t = 0f;
            int it = 0;
            while (t < 1f && it < 10000)
            {

                t += Time.deltaTime / playTime;
                it++;
                yield return null;
            }
            ResetSprite();
            m_IsCoroutineRunning = false;
        }
    }
}
