using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class DynamicInventoryDisplay : InventoryDisplay
    {
        [SerializeField] protected InventorySlot_UI slotPrefab;
        protected override void Start()
        {
            base.Start();


        }

        

        public void RefreshDynamicInventory(InventorySystem invToDisplay, int offset)
        {
            ClearSlots();
            inventorySystem = invToDisplay;
            if(inventorySystem != null)
                inventorySystem.OnInvenotrySlotChanged += UpdateSlot;
            AssignSlot(invToDisplay, offset);
        }

        public override void AssignSlot(InventorySystem invToDisplay, int offset)
        {
            ClearSlots();// Maybe redundant 

            slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

            if (invToDisplay == null) return;

            for(int i = offset; i < invToDisplay.InventorySize; i++)
            {
                var uiSlot = Instantiate(slotPrefab, transform);
                slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
                uiSlot.Init(invToDisplay.InventorySlots[i]);
                uiSlot.UpdateUISlot();
            }
        }

        private void ClearSlots()
        {
            foreach(var item in transform.Cast<Transform>())
            {
                Destroy(item.gameObject);
            }

            if (slotDictionary != null) slotDictionary.Clear();
        }

        private void OnDisable()
        {

            if (inventorySystem != null)
                inventorySystem.OnInvenotrySlotChanged -= UpdateSlot;
        }
    }
}
