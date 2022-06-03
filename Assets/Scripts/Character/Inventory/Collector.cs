using UnityEngine;

namespace ManExe
{
    [RequireComponent(typeof(InventoryHolder))]
    public class Collector : MonoBehaviour
    {
        private PlayerInventroryHolder _inventory; // TODO: make intermediate class between Inventory Holder and Player Invetory Holder with add to inventory

        private void Start()
        {
            _inventory = GetComponent<PlayerInventroryHolder>();
        }

        private void OnTriggerEnter(Collider other)
        {
            ICollectable collectable = other.GetComponent<ICollectable>();
            if (collectable != null)
            {
                if (collectable is ItemPickup)
                {
                    if (_inventory.AddToInventory(((ItemPickup)collectable).ItemData, 1))
                        collectable.Collect();
                }
            }
        }
    }
}