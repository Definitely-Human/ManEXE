using System.Collections.Generic;
using System.Linq;
using ManExe.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Events;

namespace ManExe.Entity.Inventory
{
    [System.Serializable]
    public class InventorySystem 
    {
        [SerializeField] private List<InventorySlot> _inventorySlots;


        public List<InventorySlot> InventorySlots { get => _inventorySlots; set => _inventorySlots = value; }
        public int InventorySize => InventorySlots.Count;

        public UnityAction<InventorySlot> OnInvenotrySlotChanged;
        public InventorySystem(int size){
            InventorySlots = new List<InventorySlot>(size);

            for(int i = 0; i <size; i++)
            {
                InventorySlots.Add(new InventorySlot());
            }
    	}

        public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
        {
            if(ContainsItem(itemToAdd, out List<InventorySlot> invSlot))
            {
                foreach(var slot in invSlot)
                {
                    if (slot.RoomLeftInStack(amountToAdd))
                    {
                        slot.AddToStack(amountToAdd);
                        OnInvenotrySlotChanged?.Invoke(slot);
                        return true;
                    }
                }
                
            }
            if(HasFreeSlot(out InventorySlot freeSlot))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInvenotrySlotChanged?.Invoke(freeSlot);
                return true;
            }
            return false;
        }

        public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
        {
            invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
            return invSlot == null ? false : true;
        }

        public bool HasFreeSlot(out InventorySlot freeSlot)
        {

            freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);

            return freeSlot == null ? false : true;
        }
    }
}
