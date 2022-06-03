using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ManExe
{
    /// <summary>
    /// This is a scriptable object, that defines what an item is.
    /// It could be inherited from to have branched versions of items, like potions and equipment.
    /// </summary>
    [CreateAssetMenu(fileName = "New InventoryItemData", menuName = "Scriptable/InventoryItemData", order = 0)]
    public class InventoryItemData : ScriptableObject
    {
        public int ID = -1;
        public string DisplayName;
        [TextArea(4, 4)]
        public string Description;
        public Sprite Icon;
        public int MaxStackSize;
        public GameObject ItemPrefab;
    }
}
