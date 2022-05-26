using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ManExe
{
    public class InventoryUIController : MonoBehaviour
    {
        public DynamicInventoryDisplay invetoryPanel;
        public DynamicInventoryDisplay playerBackpacPanel;

        private void Awake()
        {
            invetoryPanel.gameObject.SetActive(false);
            playerBackpacPanel.gameObject.SetActive(false);
        }
        void Update()
        {

            if (invetoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                invetoryPanel.gameObject.SetActive(false);
            }
            if (playerBackpacPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                playerBackpacPanel.gameObject.SetActive(false);
            }
        }
        private void OnEnable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInvetory;
            PlayerInventroryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
        }

        private void OnDisable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInvetory;
            PlayerInventroryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
        }

        private void DisplayInvetory(InventorySystem invToDisplay, int offset)
        {
            invetoryPanel.gameObject.SetActive(true);
            invetoryPanel.RefreshDynamicInventory(invToDisplay, offset);
        }
        private void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
        {
            playerBackpacPanel.gameObject.SetActive(true);
            playerBackpacPanel.RefreshDynamicInventory(invToDisplay, offset);
        }

    }
}
