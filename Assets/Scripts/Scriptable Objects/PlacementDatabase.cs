using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ManExe.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New PlacementDatabase", menuName = "Scriptable/PlacementDatabase", order = 0)]
    public class PlacementDatabase : ScriptableObject
    {
        [SerializeField] private List<PlacementSettings> placementDatabase;
        
        [ContextMenu("Set ID")]
        public void SetItemIDs() // This method can be executed by right clicking item name in the inspector and selecting 'Set ID'
        {
            placementDatabase = new List<PlacementSettings>();

            List<PlacementSettings> foundItems = Resources.LoadAll<PlacementSettings>("Placement").OrderBy(i => i.ID).ToList();


            List<PlacementSettings> hasIdInRange = foundItems.Where(i => i.ID > -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
            List<PlacementSettings> hasIdNotInRange = foundItems.Where(i => i.ID > -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
            List<PlacementSettings> noId = foundItems.Where(i => i.ID <= -1 ).ToList();

            int index = 0;
            for (int i = 0;i < foundItems.Count; i++)
            {
                var itemToAdd = hasIdInRange.Find(d => d.ID == i);

                if(itemToAdd != null)
                {
                    placementDatabase.Add(itemToAdd);
                }
                else if(index < noId.Count)
                {
                    noId[index].ID = i;
                    itemToAdd = noId[index];
                    index++;
                    placementDatabase.Add(itemToAdd);
                }
                foreach(var item in hasIdNotInRange)
                {
                    placementDatabase.Add(item);
                }
            }
        }
        public PlacementSettings GetItem(int id)
        {
            return placementDatabase.Find(i => i.ID == id);
        }
    }
}
