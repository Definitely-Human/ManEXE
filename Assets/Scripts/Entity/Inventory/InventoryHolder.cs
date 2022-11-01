using ManExe.SaveLoadSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ManExe.Entity.Inventory
{
    [System.Serializable]
    public abstract class InventoryHolder : MonoBehaviour
    {

        [SerializeField] protected int hotbarLength = 10; // Hotbar length
        [SerializeField] private int _inventorySize;
        [SerializeField] private InventorySystem _primaryInventorySystem;

        public int Offset => hotbarLength;

        public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; // Inv System to Display, amout to offset display by (array)

        public InventorySystem PrimaryInventorySystem { get => _primaryInventorySystem; protected set { _primaryInventorySystem = value; } }

        protected virtual void Awake()
        {
            _primaryInventorySystem = new InventorySystem(_inventorySize);
        }

        protected abstract void LoadInventory(SaveData saveData);


    }
    [System.Serializable]

    public struct InventorySaveData
    {
        public InventorySystem InvSystem;
        public Vector3 Position;
        public Quaternion Rotation;

        public InventorySaveData(InventorySystem invSystem, Vector3 position, Quaternion rotation)
        {
            this.InvSystem = invSystem;
            this.Position = position;
            this.Rotation = rotation;
        }

        public InventorySaveData(InventorySystem invSystem)
        {
            this.InvSystem = invSystem;
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
        }
    }
}
