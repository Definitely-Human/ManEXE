using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ManExe
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class UniqueID : MonoBehaviour
    {
        [SerializeField, ReadOnly] private string _id = Guid.NewGuid().ToString();
        [SerializeField] private static SerializableDictionary<string, GameObject> idDatabase 
            = new SerializableDictionary<string,GameObject>();
        public string ID => _id;

        private void OnValidate()
        {
            if (!idDatabase.ContainsKey(_id)) idDatabase.Add(_id, this.gameObject);
        }

        private void OnDestroy()
        {
            if (idDatabase.ContainsKey(_id)) idDatabase.Remove(_id);
        }

        private void Generate()
        {
            _id = Guid.NewGuid().ToString();
            idDatabase.Add(_id, this.gameObject);
        }
    }
}
