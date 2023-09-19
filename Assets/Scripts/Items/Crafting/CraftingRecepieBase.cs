using System;
using System.Collections.Generic;
using UnityEngine;


namespace DecayingEarth
{
    [Serializable]
    public struct CraftingComponent
    {
        public ItemBase Item;
        public int RequiredAmount;

        public CraftingComponent(ItemBase item, int requiredAmount)
        {
            Item = item;
            RequiredAmount = requiredAmount;
        }
    }

    [Serializable]
    [CreateAssetMenu]
    public class CraftingRecepieBase : ScriptableObject
    {
        public List<CraftingComponent> Components;
        public List<InvItem> Result;
        
    }
}
