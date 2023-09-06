using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DecayingEarth
{
    public class InvButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private InvEntryPoint m_Ivp;
        [SerializeField] private Image m_ItemIcon;
        [SerializeField] private Text m_ItemCount;

        private Image m_ButtonImage;

        [SerializeField] private Color m_HighlightColor;
        private Color m_RememberedColor;

        private int m_Id = -1;
        public int Id => m_Id;

        private void Awake()
        {
            m_ButtonImage = GetComponent<Image>();
            m_RememberedColor = m_ButtonImage.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //������� ���, ���� ����������� Tooltip ��������
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //������� ���, ����� ��������� Tooltip ��������
        }

        // ����� - 0, ������ - 1
        public void OnPointerClick(PointerEventData eventData)
        {
            m_Ivp.MakeActionDependingOnClickType((int)eventData.button, m_Id);
        }

        /// <summary>
        /// �������������� ������
        /// </summary>
        /// <param name="id">�� ������, ����� �� ������ ���������</param>
        /// <param name="inv">����� �����, � ������� ����� ��������� ������ ��� �������</param>
        public void Initiate(int id, InvEntryPoint inv)
        {
            m_Id = id;
            m_Ivp = inv;
        }

        /// <summary>
        /// ��������� ������ � �� ����������
        /// </summary>
        public void UpdateButtonGraphics()
        {
            if (m_Ivp.Inventory == null)
            {
                Debug.LogError("Inventory wasn't set upon accessing it! Object: " + m_Ivp.gameObject);
                return;
            }

            if (Id == -1) return;

            if (m_ItemIcon == null)
            {
                Debug.LogError("Item Icon object isn't set on: " + gameObject);
            }

            if (m_Ivp.Inventory.Items[m_Id].Item != null && m_ItemIcon != null) m_ItemIcon.sprite = m_Ivp.Inventory.Items[m_Id].Item.Icon;
            if (m_Ivp.Inventory.Items[m_Id].Item == null && m_ItemIcon != null) m_ItemIcon.sprite = Singleton_PrefabLibrary.Instance.EmptySprite;
            if (m_Ivp.Inventory.Items[m_Id].StackCount > 1 && m_ItemCount != null) m_ItemCount.text = m_Ivp.Inventory.Items[m_Id].StackCount.ToString();
            if (m_Ivp.Inventory.Items[m_Id].StackCount <= 1 && m_ItemCount != null) m_ItemCount.text = "";

        }

        /// <summary>
        /// ������ ���� ������ �� "����������"
        /// </summary>
        public void HighlightButton()
        {
            m_ButtonImage.color = m_HighlightColor;
        }

        /// <summary>
        /// ������ ���� ������ �� �����������
        /// </summary>
        public void UnHighlightButton()
        {
            m_ButtonImage.color = m_RememberedColor;
        }
    }
}
