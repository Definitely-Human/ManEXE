using System;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.Entity.Inventory
{
    [System.Serializable]
    public class InventorySlot : ISerializationCallbackReceiver
    {
        [NonSerialized] private InventoryItemData _itemData;
        [SerializeField] private int _itemId = -1; // Might be redundant 
        [SerializeField] private int _stackSize;
    	

        public int StackSize { get => _stackSize; }
        public InventoryItemData ItemData { get => _itemData; }

        public InventorySlot(InventoryItemData source, int amount)
        {
            _itemData = source;
            _itemId = ItemData.ID;
            _stackSize = amount;
        }

        public InventorySlot()
        {
            ClearSlot();
        }

        public void AssignItem(InventorySlot invSlot)
        {
            if (_itemData == invSlot.ItemData) AddToStack(invSlot.StackSize);
            else
            {
                _itemData = invSlot.ItemData;
                _itemId = ItemData.ID;
                _stackSize = 0;
                AddToStack(invSlot.StackSize);
            }
        }

        public void ClearSlot()
        {
            _itemData = null;
            _itemId = -1;
            _stackSize = -1;
        }

        public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
        {
            amountRemaining = _itemData.MaxStackSize - StackSize;
            return RoomLeftInStack(amountToAdd);
        }

        public void UpdateInventorySlot(InventoryItemData data, int amount)
        {
            _itemData = data;
            _itemId = ItemData.ID;
            _stackSize = amount;
        }

        public bool RoomLeftInStack(int amountToAdd)
        {
            if (_stackSize + amountToAdd <= _itemData.MaxStackSize) return true;
            else return false;
        }

        public void AddToStack(int amount)
        {
            _stackSize += amount;
        }

        public void RemoveFromStack(int amount)
        {
            _stackSize -= amount;
        }

        public bool SplitStack(out InventorySlot splitStack)
        {
            if(StackSize <= 1)
            {
                splitStack = null;
                return false;
            }
            int halfStack = Mathf.RoundToInt(_stackSize / 2);
            RemoveFromStack(halfStack);

            splitStack = new InventorySlot(ItemData, halfStack);
            return true;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            if (_itemId == -1) return;
            var db = Resources.Load<Database>("ItemIdDatabase");
            _itemData = db.GetItem(_itemId);
        }
    }
}
