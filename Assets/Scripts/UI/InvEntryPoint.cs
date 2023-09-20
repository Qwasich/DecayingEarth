using UnityEngine;
using UnityEngine.Events;

namespace DecayingEarth
{
    public class InvEntryPoint : MonoBehaviour
    {
        public UnityAction ReadyToUpdate;

        [SerializeField] private Inventory m_Inventory;
        /// <summary>
        /// Инвертарь, прикрепленный ко входной точке.
        /// </summary>
        public Inventory Inventory => m_Inventory;

        [SerializeField] private InvButton[] m_ButtonArray;
        public InvButton[] ButtonArray => m_ButtonArray;

        private Singleton_MouseItemHolder mIh => Singleton_MouseItemHolder.Instance; 

        public void Start()
        {
            InitiateInvPoint();
            ReadyToUpdate?.Invoke();
        }

        /// <summary>
        /// Прикрепляет инвентарь ко входной точке.
        /// </summary>
        /// <param name="inv"></param>
        public void SetInventory(Inventory inv) => m_Inventory = inv;

        public void InitiateInvPoint()
        {
            if(m_Inventory == null) return;
            for (int i = 0; i < m_ButtonArray.Length; i++)
            {
                m_ButtonArray[i].Initiate(i, this);
                m_ButtonArray[i].UpdateButtonGraphics();
            }
        }

        /// <summary>
        /// Обновляет все кнопки, привязанные к входной точке инвентаря.
        /// </summary>
        public void UpdateAllButtons()
        {
            if (m_Inventory == null)
            {
                Debug.LogError("Inventory isn't set on entry point while attempting a call for UI update on object: " + gameObject);
                return;
            }

            for (int i = 0; i < m_ButtonArray.Length; i++) m_ButtonArray[i].UpdateButtonGraphics();
        }

        /// <summary>
        /// Обновляет кнопку с определенным индексом.
        /// </summary>
        /// <param name="index">Индекс обновляемой кнопки. Равен ячейке инвентаря.</param>
        public void UpdateButton(int index)
        {
            if (m_Inventory == null)
            {
                Debug.LogError("Inventory isn't set on entry point while attempting a call for UI update on object: " + gameObject);
                return;
            }
            m_ButtonArray[index].UpdateButtonGraphics();
        }

        /// <summary>
        /// Взаимодействует с инвентарем в ячейке, в зависимости от типа клика.
        /// </summary>
        /// <param name="clickType">Тип клика - 0 или 1. Остальные игнорируются.</param>
        /// <param name="bId">Номер ячейки инвентаря.</param>
        public void MakeActionDependingOnClickType(int clickType, int bId)
        {
            if (m_Inventory == null || Singleton_SessionData.Instance.IsInventoryHidden) return;
            if (mIh.HandItem.Item == null && m_Inventory.Items[bId].Item == null) return;
            if (m_Inventory.Items.Count <= bId)
            {
                Debug.LogError("Attempting to reach unreachable inv ID in inventory: " + m_Inventory.gameObject + " with ID of " + bId + ". Consider expanding inventory or reducing button count.");
                return;
            }

            if (clickType == 0)
            {
                if (mIh.HandItem.Item == null && m_Inventory.Items[bId].Item != null)
                {
                    mIh.GrabItem(m_Inventory.Items[bId]);
                    m_Inventory.RemoveItemCompletely(bId);
                    m_ButtonArray[bId].UpdateButtonGraphics();
                    ReadyToUpdate?.Invoke();
                    return;
                }

                if (mIh.HandItem.Item != null && m_Inventory.Items[bId].Item == null)
                {
                    m_Inventory.AddNewItem(bId, mIh.HandItem);
                    mIh.RemoveItem();
                    m_ButtonArray[bId].UpdateButtonGraphics();
                    ReadyToUpdate?.Invoke();
                    return;
                }

                if (mIh.HandItem.Item != null && m_Inventory.Items[bId].Item != null)
                {
                    if(m_Inventory.Items[bId].Item != mIh.HandItem.Item)
                    {
                        InvItem it = m_Inventory.Items[bId];
                        m_Inventory.AddNewItem(bId, mIh.HandItem);
                        mIh.RemoveItem();
                        mIh.GrabItem(it);
                        m_ButtonArray[bId].UpdateButtonGraphics();
                        ReadyToUpdate?.Invoke();
                        return;
                    }
                    else
                    {
                        m_Inventory.IncreaseItemCount(bId, mIh.HandItem.StackCount, out int count);
                        mIh.RemoveItem();
                        if (count != 0) mIh.GrabItem(new InvItem(m_Inventory.Items[bId].Item, count));
                        m_ButtonArray[bId].UpdateButtonGraphics();
                        ReadyToUpdate?.Invoke();
                        return;
                    }
                }
            }

            if(clickType == 1)
            {
                if (mIh.HandItem.Item == null && m_Inventory.Items[bId].Item != null)
                {
                    mIh.GrabItem(new InvItem(m_Inventory.Items[bId].Item, 1));
                    m_Inventory.DecreaseItemCount(bId, 1);
                    m_ButtonArray[bId].UpdateButtonGraphics();
                    ReadyToUpdate?.Invoke();
                    return;
                }

                if (mIh.HandItem.Item != null && m_Inventory.Items[bId].Item == null)
                {
                    m_Inventory.AddNewItem(bId, new InvItem(mIh.HandItem.Item,1));
                    mIh.DecreaseHandItemByNumber(1);
                    m_ButtonArray[bId].UpdateButtonGraphics();
                    ReadyToUpdate?.Invoke();
                    return;
                }

                if (mIh.HandItem.Item != null && m_Inventory.Items[bId].Item != null)
                {
                    if (mIh.HandItem.Item != m_Inventory.Items[bId].Item) return;
                    m_Inventory.DecreaseItemCount(bId,1);
                    mIh.IncreaseHandItemByNumber();
                    m_ButtonArray[bId].UpdateButtonGraphics();
                    ReadyToUpdate?.Invoke();
                    return;
                }
            }
        }

        /// <summary>
        /// Подбирает предмет. Если что идет не так - редактирует не подобраный стак с остатками предметов.
        /// <param name="pItem"> Подбираемый предмет </param>
        public void PickUpItem(PhysicalItem pItem)
        {
            if (pItem.IsPickableUp == false) return;
            //Debug.Log("Reached picking up");
            int oldcount = pItem.HandledItem.StackCount;
            int count = oldcount;
            int firstEmpty = -1;

            for (int i = 0; i < Inventory.Items.Count; i++)
            {
                if (firstEmpty == -1 && m_Inventory.Items[i].Item == null) firstEmpty = i;

                if (m_Inventory.Items[i].Item == pItem.HandledItem.Item)
                {
                    if (m_Inventory.Items[i].Item.MaxStackCount == m_Inventory.Items[i].StackCount) continue;
                    //Debug.Log("found Item To Edit");
                    bool upd = false;
                    if (m_Inventory.IncreaseItemCount(i, count, out count)) upd = true;
                    if (upd && m_ButtonArray[i].gameObject.activeSelf == true) m_ButtonArray[i].UpdateButtonGraphics();
                    if (count == 0) break;
                }
            }
            if (count != 0 && firstEmpty != -1)
            {
                //Debug.Log("Created NewI tem");
                m_Inventory.AddNewItem(firstEmpty, new InvItem(pItem.HandledItem.Item, count));
                m_ButtonArray[firstEmpty].UpdateButtonGraphics();
                ReadyToUpdate?.Invoke();
                count = 0;
            }
            if (firstEmpty == -1 && count == oldcount) return;
            pItem.SetItemCount(count);

        }

    }
}
