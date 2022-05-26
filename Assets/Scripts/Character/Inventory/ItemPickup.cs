using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ManExe
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(UniqueID))]
    public class ItemPickup : MonoBehaviour
    {

        public float PickUpRadius = 1f;
        public InventoryItemData ItemData;

        private SphereCollider _myCollider;

        [SerializeField] private ItemPickUpSaveData itemSaveData;
        private string id;

        private void Awake()
        {
            id = GetComponent<UniqueID>().ID;

            SaveLoadManager.OnLoadGame += LoadGame;
            itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);

            _myCollider = GetComponent<SphereCollider>();
            _myCollider.isTrigger = true;
            _myCollider.radius = PickUpRadius;
        }

        private void Start()
        {
            SaveGameManager.data.activeItems.Add(id, itemSaveData);
        }

        private void LoadGame(SaveData data)
        {
            if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var inventory = other.transform.GetComponent<PlayerInventroryHolder>();
            if (!inventory) return;

            if (inventory.AddToInventory(ItemData, 1))
            {
                SaveGameManager.data.collectedItems.Add(id);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
            SaveLoadManager.OnLoadGame -= LoadGame;
        }
    }
    [System.Serializable]
    public struct ItemPickUpSaveData
    {
        public InventoryItemData ItemData;
        public Vector3 Position;
        public Quaternion Rotation;

        public ItemPickUpSaveData(InventoryItemData itemData, Vector3 position, Quaternion rotation)
        {
            ItemData = itemData;
            Position = position;
            Rotation = rotation;
        }
    }
}

