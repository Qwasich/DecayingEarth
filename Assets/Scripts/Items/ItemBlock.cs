using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScorgedEarth
{
    [CreateAssetMenu]
    public class ItemBlock : ItemBase
    {
         protected override ItemType m_ItemType => ItemType.Block;

        [SerializeField] protected TileBlockBase m_TileBlock;
    }
}
