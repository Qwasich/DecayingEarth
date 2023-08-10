using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScorgedEarth
{
    [CreateAssetMenu]
    public class ItemMaterial : ItemBase
    {
        [SerializeField] protected override ItemType m_ItemType => ItemType.Material;


    }
}
