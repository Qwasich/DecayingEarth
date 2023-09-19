using System;
using System.Collections.Generic;
using UnityEngine;


namespace DecayingEarth
{
    [Serializable]
    public struct CraftingComponent
    {
        public ItemBase Item;
        public int Amount;

        public CraftingComponent(ItemBase item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    [Serializable]
    [CreateAssetMenu]
    public class CraftingRecipeBase : ScriptableObject
    {
        public List<CraftingComponent> Components;
        public CraftingComponent Result;
        
    }
}
