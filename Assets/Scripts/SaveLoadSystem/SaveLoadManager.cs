using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace ManExe.SaveLoadSystem
{

    public static class SaveLoadManager 
    {
        public static SaveData CurrentSaveData = new SaveData();
        public static UnityAction OnSaveGame;
        public static UnityAction<SaveData> OnLoadGame;

        public const string directory = "/SaveData/";

        internal static void DeleteSaveData()
        {
            string fullPath = Application.persistentDataPath + directory + fileName;
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        public const string fileName = "SaveGame.sav";

        public static bool Save(SaveData data)
        {
            OnSaveGame?.Invoke();

            string dir = Application.persistentDataPath + directory;

            GUIUtility.systemCopyBuffer = dir;

            if (!Directory.Exists(dir)){
                Directory.CreateDirectory(dir);
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(dir + fileName, json);

            Debug.Log("Game Saved");

            return true;
        }

        public static SaveData Load()
        {
            string fullPath = Application.persistentDataPath + directory + fileName;
            SaveData data = new SaveData();

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                data = JsonUtility.FromJson<SaveData>(json);

                OnLoadGame?.Invoke(data);

                Debug.Log("Save file loaded succesfully.");
            }
            else
            {
                Debug.Log("Save file does not exist.");
            }
            return data;
        }
    
    }
}
