using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace DecayingEarth
{
    public class Singleton_MouseItemHolder : MonoSingleton<Singleton_MouseItemHolder>
    {
        [SerializeField] private GameObject m_TooltipItem;
        [SerializeField] private Text m_TooltipText;
        [SerializeField] private GameObject m_MouseObject;
        [SerializeField] private Image m_Icon;
        [SerializeField] private Text m_Text;
        private InvItem m_HandItem;
        public InvItem HandItem => m_HandItem;

        public void Update()
        {
            if (m_MouseObject.activeInHierarchy == true) m_MouseObject.transform.position = Input.mousePosition;
            if (m_TooltipItem.activeInHierarchy == true) m_TooltipItem.transform.position = Input.mousePosition;
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
            HideTooltip();

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
        public bool IncreaseHandItemByNumber(int count = 1)
        {
            if (m_HandItem.Item == null) return false;

            if (m_HandItem.StackCount + count > m_HandItem.Item.MaxStackCount) return false;
            m_HandItem = new InvItem(m_HandItem.Item, m_HandItem.StackCount + count);

            UpdateHandVisual();
            return true;
        }

        /// <summary>
        /// Обновляет изображение и текст, прикрепленные к руке.
        /// </summary>
        public void UpdateHandVisual()
        {
            if (m_HandItem.Item != null && m_Icon != null) m_Icon.sprite = m_HandItem.Item.Icon;
            if (m_Text != null && m_HandItem.StackCount > 1) m_Text.text = m_HandItem.StackCount.ToString();
            if (m_Text != null && m_HandItem.StackCount <= 1) m_Text.text = "";
        }

        /// <summary>
        /// Показывает окошко с Tooltip
        /// </summary>
        /// <param name="text">Текст, который надо вывести на Tooltip</param>
        public void ShowTooltip(string text)
        {
            if(m_TooltipItem != null && HandItem.Item == null) m_TooltipItem.SetActive(true);
            UpdateTooltipText(text);
        }

        /// <summary>
        /// Обновляет текст Tooltip переданным
        /// </summary>
        /// <param name="text">Текст, который надо вывести на Tooltip</param>
        public void UpdateTooltipText(string text)
        {
            if (m_TooltipText != null && HandItem.Item == null) m_TooltipText.text = text;
        }

        /// <summary>
        /// Прячет Tooltip
        /// </summary>
        public void HideTooltip()
        {
            if (m_TooltipItem != null) m_TooltipItem.SetActive(false);
        }


    }
}
