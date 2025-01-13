using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DecayingEarth
{
    [CreateAssetMenu]
    public class ItemMaterial : ItemBase, UseItem
    {
        [SerializeField] protected override ItemType m_ItemType => ItemType.Material;

        public int UseItem(int clickType, Creature creature, InvEntryPoint inventory)
        {
            return 0;
        }

    }
}
