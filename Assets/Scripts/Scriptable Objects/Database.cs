using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ManExe.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Database", menuName = "Scriptable/Database", order = 0)]
    public class Database : ScriptableObject
    {
        [SerializeField] private List<InventoryItemData> _itemDatabase;
        
        [ContextMenu("Set ID")]
        public void SetItemIDs()
        {
            _itemDatabase = new List<InventoryItemData>();

            List<InventoryItemData> foundItems = Resources.LoadAll<InventoryItemData>("InventoryItemData").OrderBy(i => i.ID).ToList();


            List<InventoryItemData> hasIdInRange = foundItems.Where(i => i.ID > -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
            List<InventoryItemData> hasIdNotInRange = foundItems.Where(i => i.ID > -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
            List<InventoryItemData> noId = foundItems.Where(i => i.ID <= -1 ).ToList();

            int index = 0;
            for (int i = 0;i < foundItems.Count; i++)
            {
                InventoryItemData itemToAdd;
                itemToAdd = hasIdInRange.Find(d => d.ID == i);

                if(itemToAdd != null)
                {
                    _itemDatabase.Add(itemToAdd);
                }
                else if(index < noId.Count)
                {
                    noId[index].ID = i;
                    itemToAdd = noId[index];
                    index++;
                    _itemDatabase.Add(itemToAdd);
                }
                foreach(var item in hasIdNotInRange)
                {
                    _itemDatabase.Add(item);
                }
            }
        }
        public InventoryItemData GetItem(int id)
        {
            return _itemDatabase.Find(i => i.ID == id);
        }
    }
}
