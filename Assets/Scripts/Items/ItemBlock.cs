using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class ItemBlock : ItemBase, UseItem
    {
         protected override ItemType m_ItemType => ItemType.Block;

        [SerializeField] protected TileBlockBase m_TileBlock;
        [SerializeField] protected float m_Distance = 2;

        public int UseItem(int clickType)
        {
            if (Singleton_SessionData.Instance.IsLastClickWasOnCanvas) return 0;

            if (clickType == 0)
            {
                if (m_TileBlock != null)
                {
                    bool b = Singleton_BlockEditor.Instance.PlaceWallBlock(m_TileBlock,m_Distance);
                    if (!b) return 0;
                }
                else
                {
                    Debug.LogError("Tile block isn't set on the item " + Name + ", check the corresponding prefab");
                    return 0;
                }
                return StackDecreaseAfterUse;
            }

            return 0;

        }
    }
}
