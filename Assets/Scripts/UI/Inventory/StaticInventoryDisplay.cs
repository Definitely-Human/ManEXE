using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class StaticInventoryDisplay : InventoryDisplay
    {
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private InventorySlot_UI[] slots;

        private void OnEnable()
        {
            PlayerInventroryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
        }

        private void OnDisable()
        {
            PlayerInventroryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
        }

        private void RefreshStaticDisplay()
        {
            if (_inventoryHolder != null)
            {
                inventorySystem = _inventoryHolder.PrimaryInventorySystem;
                inventorySystem.OnInvenotrySlotChanged += UpdateSlot;
            }
            else Debug.LogWarning($"No inventory assigned to {this.gameObject}");

            AssignSlot(inventorySystem, 0);
        }

        protected override void Start()
        {
            base.Start();

            RefreshStaticDisplay();
        }
        public override void AssignSlot(InventorySystem invToDisplay, int offset)
        {
            slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();


            for (int i = 0; i< _inventoryHolder.Offset; i++)
            {
                slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
                slots[i].Init(inventorySystem.InventorySlots[i]);
            }
        }

        
    }
}
