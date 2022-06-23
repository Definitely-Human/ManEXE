using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class SaveLoadUI : MonoBehaviour
    {
        private InputReader _inputReader;
        [SerializeField] private GameObject _saveLoadMenuContainer;

        private void Awake()
        {
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");
        }

        private void Start()
        {
            
            _saveLoadMenuContainer.SetActive(false);
        }

        private void OnEnable()
        {
            _inputReader.SaveLoadMenuEvent += OnSaveLoadMenuEvent;
        }
        
        private void OnDisable()
        {
            _inputReader.SaveLoadMenuEvent -= OnSaveLoadMenuEvent;
        }

        private void OnSaveLoadMenuEvent()
        {
            _saveLoadMenuContainer.SetActive(!_saveLoadMenuContainer.activeInHierarchy);
        }
    }
}
