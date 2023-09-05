using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScourgedEarth
{
    [CreateAssetMenu]
    public class ItemMaterial : ItemBase, UseItem
    {
        [SerializeField] protected override ItemType m_ItemType => ItemType.Material;

        public int UseItem(int clickType)
        {
            return 0;
        }

    }
}
