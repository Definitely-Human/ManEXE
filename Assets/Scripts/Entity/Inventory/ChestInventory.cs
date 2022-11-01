using ManExe.Entity.Interactable;
using ManExe.Interfaces;
using ManExe.SaveLoadSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ManExe.Entity.Inventory
{

    [RequireComponent(typeof(UniqueID))]
    public class ChestInventory : InventoryHolder, IInteractable
    {
        private string _id;
        public UnityAction<IInteractable> OnInteractionComplete { get; set; }
        protected override void Awake()
        {
            base.Awake();
            SaveLoadManager.OnLoadGame += LoadInventory;
        }

        private void Start()
        {
            var chestSaveData = new InventorySaveData(PrimaryInventorySystem, transform.position, transform.rotation);
            _id = GetComponent<UniqueID>().ID;
            SaveGameManager.data.chestDictionary.Add(_id,chestSaveData);
        }

        protected override void LoadInventory(SaveData data)
        {
            
            if (data.chestDictionary.TryGetValue(_id,out InventorySaveData chestData))
            {
                PrimaryInventorySystem = chestData.InvSystem;
                transform.rotation = chestData.Rotation;
                transform.position = chestData.Position;
            }
        }

        private void OnDestroy()
        {
            SaveLoadManager.OnLoadGame -= LoadInventory;

        }

        public void Interact(Interactor interactor, out bool interactSuccessful)
        {
            OnDynamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, 0);
            interactSuccessful = true;
        }

        public void EndInteraction()
        {
            throw new System.NotImplementedException();
        }
    }

    
}
