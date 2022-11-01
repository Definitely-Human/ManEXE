using ManExe.SaveLoadSystem;
using ManExe.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Events;

namespace ManExe.Entity.Inventory
{
    [RequireComponent(typeof(UniqueID))]
    public class PlayerInventroryHolder : InventoryHolder
    {
        public static UnityAction OnPlayerInventoryChanged;
        public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;
        private InputReader _inputReader;
        
        protected override void Awake()
        {
            base.Awake();
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");
        }
        
        private void Start()
        {
            SaveGameManager.data.playerInventory = new InventorySaveData(PrimaryInventorySystem);
        }


        private void OnEnable()
        {
            SaveLoadManager.OnLoadGame += LoadInventory;
            _inputReader.OpenInventoryEvent += OnOpenBackpackInventory;
        }

        private void OnOpenBackpackInventory()
        {
            OnPlayerInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, hotbarLength);
        }

        private void OnDisable()
        {
            SaveLoadManager.OnLoadGame -= LoadInventory;
            _inputReader.OpenInventoryEvent -= OnOpenBackpackInventory;
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