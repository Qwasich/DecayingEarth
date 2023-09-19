using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DecayingEarth
{
    public class CraftingComponentButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image m_ItemIcon;
        [SerializeField] private Text m_ItemCount;

        private Image m_ButtonImage;
        private UIRecipeCreator m_Creator;

        [SerializeField] private Color m_HighlightColor;
        private Color m_RememberedColor;

        private CraftingComponent m_AttachedItem;
        /// <summary>
        /// ������������� � ������ ������
        /// </summary>
        public CraftingComponent AttachedItem => m_AttachedItem;

        [SerializeField] private bool m_IsInteractable = false;
        /// <summary>
        /// ���� True - �� ������ ����� ������
        /// </summary>
        public bool IsInteractable => m_IsInteractable;

        /// <summary>
        /// ������ ����������� ������������������ � �������.
        /// </summary>
        /// <param name="i">����</param>
        public void SetButtonInteractability (bool i) => m_IsInteractable = i;


        private void Awake()
        {
            m_ButtonImage = GetComponent<Image>();
            m_RememberedColor = m_ButtonImage.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var de = m_AttachedItem.Item;

            if (de == null || Singleton_SessionData.Instance.IsInventoryHidden) return;

            Singleton_MouseItemHolder.Instance.ShowTooltip(InvButton.FormatItemToTooltip(de));

        }


        public void OnPointerExit(PointerEventData eventData) => Singleton_MouseItemHolder.Instance.HideTooltip();

        // ����� - 0, ������ - 1
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable) return;
            m_Creator.StartCrafting();
        }

        /// <summary>
        /// �������������� ������
        /// </summary>
        /// <param name="item">�������, ������������� � ������</param>
        /// <param name="crp">����� �����, � ������� ����� ��������� ������ ��� �������</param>
        public void Initiate(CraftingComponent component, UIRecipeCreator cr)
        {
            m_AttachedItem = component;
            m_Creator = cr;
        }

        /// <summary>
        /// ��������� ������ � �� ����������
        /// </summary>
        public void UpdateButtonGraphics()
        {

            if (m_ItemIcon == null)
            {
                Debug.LogError("Item Icon object isn't set on: " + gameObject);
            }

            if (m_AttachedItem.Item != null && m_ItemIcon != null) m_ItemIcon.sprite = m_AttachedItem.Item.Icon;
            if (m_AttachedItem.Amount > 1 && m_ItemCount != null) m_ItemCount.text = m_AttachedItem.Amount.ToString();
            if (m_AttachedItem.Amount <= 1 && m_ItemCount != null) m_ItemCount.text = "";

        }

        /// <summary>
        /// ������ ������ ������
        /// </summary>
        public void DimButton()
        {
            m_ButtonImage.color = m_HighlightColor;
            m_ItemIcon.color = m_HighlightColor;
        }

        /// <summary>
        /// ������ ���� ������ �� �����������
        /// </summary>
        public void UndimButton()
        {
            m_ButtonImage.color = m_RememberedColor;
            m_ItemIcon.color = Color.white;
        }
    }
}
