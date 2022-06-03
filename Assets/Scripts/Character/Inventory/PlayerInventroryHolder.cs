using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ManExe
{
    [RequireComponent(typeof(UniqueID))]
    public class PlayerInventroryHolder : InventoryHolder
    {
        public static UnityAction OnPlayerInventoryChanged;
        public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;

        private void Start()
        {
            SaveGameManager.data.playerInventory = new InventorySaveData(PrimaryInventorySystem);
        }

        private void Update()
        {
            if (Keyboard.current.bKey.wasPressedThisFrame) OnPlayerInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, hotbarLength);
        }

        private void OnEnable()
        {
            SaveLoadManager.OnLoadGame += LoadInventory;
        }

        private void OnDisable()
        {
            SaveLoadManager.OnLoadGame -= LoadInventory;
        }

        protected override void LoadInventory(SaveData data)
        {
            string id = GetComponent<UniqueID>().ID;
            if (data.playerInventory.InvSystem != null)
            {
                PrimaryInventorySystem = data.playerInventory.InvSystem;
                OnPlayerInventoryChanged?.Invoke();
            }
        }

        public bool AddToInventory(InventoryItemData data, int amount)
        {
            if (PrimaryInventorySystem.AddToInventory(data, amount))
            {
                return true;
            }
            return false;
        }
    }
}