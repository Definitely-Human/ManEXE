using UnityEngine;

namespace ManExe
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(UniqueID))]
    public class ItemPickup : MonoBehaviour, ICollectable
    {
        public float PickUpRadius = 1f;
        public InventoryItemData ItemData;

        private float _rotationSpeed = 20;
        private SphereCollider _myCollider;

        [SerializeField] private ItemPickUpSaveData itemSaveData;
        private string id;

        public static event HandleItemCollected OnItemCollected;

        public delegate void HandleItemCollected(InventoryItemData itemData);

        private void Awake()
        {
            SaveLoadManager.OnLoadGame += LoadGame;
            itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);

            _myCollider = GetComponent<SphereCollider>();
            _myCollider.isTrigger = true;
            _myCollider.radius = PickUpRadius;
        }

        private void Start()
        {
            id = GetComponent<UniqueID>().ID;
            SaveGameManager.data.activeItems.Add(id, itemSaveData);
        }

        private void Update()
        {
            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
        }

        private void LoadGame(SaveData data)
        {
            if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
            SaveLoadManager.OnLoadGame -= LoadGame;
        }

        public InventoryItemData Collect()
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(gameObject);
            OnItemCollected?.Invoke(ItemData);
            // Cant use static event to add item to players inventory because it will add to inventory of every listener
            // Maybe there is a better way to do this.
            return ItemData;
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