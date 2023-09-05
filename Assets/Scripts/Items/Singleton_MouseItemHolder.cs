using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace ScourgedEarth
{
    public class Singleton_MouseItemHolder : MonoSingleton<Singleton_MouseItemHolder>
    {
        [SerializeField] private GameObject m_MouseObject;
        [SerializeField] private Image m_Icon;
        [SerializeField] private Text m_Text;
        private InvItem m_HandItem;
        public InvItem HandItem => m_HandItem;

        public void Update()
        {
            if (m_MouseObject.activeInHierarchy == false) return;
            m_MouseObject.transform.position = Input.mousePosition;
        }

        /// <summary>
        /// Добавляет предмет в ячейку руки.
        /// </summary>
        /// <param name="item">Добавляемый предмет</param>
        /// <returns>Возвращает False если комманда неуспешна.</returns>
        public bool GrabItem(InvItem item)
        {
            if (item.Item == null || m_HandItem.Item != null) return false;
            m_HandItem = item;
            m_MouseObject.SetActive(true);
            UpdateHandVisual();

            return true;
        }

        /// <summary>
        /// Убирает предмет из руки.
        /// </summary>
        /// <returns>Возвращает False если комманда неуспешна.</returns>
        public bool RemoveItem()
        {
            if( m_HandItem.Item == null)
            {
                return false;
            }
            m_HandItem = new InvItem();
            m_MouseObject.SetActive(false);

            return true;
        }

        /// <summary>
        /// Роняет предмет на землю, создавая новый объект из префаба
        /// </summary>
        /// <returns>Возвращает False если комманда неуспешна.</returns>
        public bool DropItem()
        {
            if (m_HandItem.Item == null)
            {
                return false;
            }

            Vector2 pos = Camera.main.transform.position;
            GameObject item = Instantiate(Singleton_PrefabLibrary.Instance.DummyItemPrefab,pos,Quaternion.identity);
            PhysicalItem pi = item.GetComponent<PhysicalItem>();
            pi.InitiateItem(m_HandItem.Item, m_HandItem.StackCount, false);

            m_HandItem = new InvItem();
            m_MouseObject.SetActive(false);

            return true;
        }

        /// <summary>
        /// Уменьшает стак предмета в руке на 1.
        /// </summary>
        /// <returns>Возвращает False если комманда неуспешна.</returns>
        public bool DecreaseHandItemByNumber(int amount)
        {
            if (m_HandItem.Item == null || m_HandItem.StackCount < amount) return false;

            if (m_HandItem.StackCount - amount > 0)
            {
                m_HandItem = new InvItem(m_HandItem.Item, m_HandItem.StackCount - amount);
                UpdateHandVisual();
                return true;
            }

            m_HandItem = new InvItem();
            m_MouseObject.SetActive(false);
            return true;
        }

        /// <summary>
        /// Увеличивает количество предметов в руке на 1, если позволяет размер стака.
        /// </summary>
        /// <returns>Возвращает False если комманда неуспешна.</returns>
        public bool IncreaseHandItemByOne()
        {
            if (m_HandItem.Item == null) return false;

            if (m_HandItem.StackCount + 1 > m_HandItem.Item.MaxStackCount) return false;
            m_HandItem = new InvItem(m_HandItem.Item, m_HandItem.StackCount + 1);

            UpdateHandVisual();
            return true;
        }

        /// <summary>
        /// Обновляет изображение и текст, прикрепленные к руке.
        /// </summary>
        public void UpdateHandVisual()
        {
            if (m_Icon != null) m_Icon.sprite = m_HandItem.Item.Icon;
            if (m_Text != null && m_HandItem.StackCount > 1) m_Text.text = m_HandItem.StackCount.ToString();
            if (m_Text != null && m_HandItem.StackCount <= 1) m_Text.text = "";
        }


    }
}
