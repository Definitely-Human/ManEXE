using UnityEngine;

namespace ManExe.SaveLoadSystem
{
    public class SaveGameManager : MonoBehaviour
    {
        public static SaveData data;

        private void Awake()
        {
            data = new SaveData();
            SaveLoadManager.OnLoadGame += LoadData;
        }

        public void DeleteData()
        {
            SaveLoadManager.DeleteSaveData();
        }

        public static void SaveData()
        {
            var saveData = data;
            SaveLoadManager.Save(saveData);
        }

        public static void LoadData(SaveData _data)
        {
            data = _data;
        }

        public static void TryLoadData()
        {
            SaveLoadManager.Load();
        }
    }
}
