using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace ManExe
{
    public abstract class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private MouseItemData _mouseInventoryItem;
        protected InventorySystem inventorySystem;
        protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;

        public InventorySystem InventorySystem => inventorySystem;
        public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

        public abstract void AssignSlot(InventorySystem invToDisplay, int offset);

        protected virtual void Start()
        {

        }

        protected virtual void UpdateSlot(InventorySlot updatedSlot)
        {
            foreach(var slot in SlotDictionary)
            {
                if(slot.Value == updatedSlot)
                {
                    slot.Key.UpdateUISlot(updatedSlot);
                }
            }
        }

        public void SlotClicked(InventorySlot_UI clickedUISlot)
        {
            bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
            if(clickedUISlot.AssignedInventorySlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData == null)
            {
                if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
                {
                    _mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                    clickedUISlot.UpdateUISlot();
                    return;
                }
                else
                {
                    _mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                    clickedUISlot.ClearSlot();
                    return;
                }
            }

            if(clickedUISlot.AssignedInventorySlot.ItemData == null && _mouseInventoryItem.AssignedInventorySlot.ItemData != null)
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                _mouseInventoryItem.ClearSlot();
                return;
            }

            if(clickedUISlot.AssignedInventorySlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData != null)
            {
                bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == _mouseInventoryItem.AssignedInventorySlot.ItemData;
                if (isSameItem && clickedUISlot.AssignedInventorySlot.RoomLeftInStack(_mouseInventoryItem.AssignedInventorySlot.StackSize)){
                    clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                    clickedUISlot.UpdateUISlot();
                    _mouseInventoryItem.ClearSlot();
                }
                else if(isSameItem && 
                    !clickedUISlot.AssignedInventorySlot.RoomLeftInStack(_mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
                {
                    if (leftInStack < 1) SwapSlots(clickedUISlot); // Stack is full so swap items
                    else // Slot is not max so take what is needed
                    {
                        int remainingOnMouse = _mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                        clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                        clickedUISlot.UpdateUISlot();

                        var newItem = new InventorySlot(_mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                        _mouseInventoryItem.ClearSlot();
                        _mouseInventoryItem.UpdateMouseSlot(newItem);
                    }

                }
                else if(!isSameItem)
                {
                    SwapSlots(clickedUISlot);
                }
                return;
            }
        }

        private void SwapSlots(InventorySlot_UI clickedUISlot)
        {
            var clonedSlot = new InventorySlot(_mouseInventoryItem.AssignedInventorySlot.ItemData, _mouseInventoryItem.AssignedInventorySlot.StackSize);
            _mouseInventoryItem.ClearSlot();
            _mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

            clickedUISlot.ClearSlot();
            clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
            clickedUISlot.UpdateUISlot();
        }
    }
}
