using ManExe.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ManExe.Entity.Interactable
{
    public class MouseScreenRayProvider : MonoBehaviour, IRayProvider
    {
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public Ray CreateRay()
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();   
            return _mainCamera.ScreenPointToRay(mousePos);
        }
            
    }

    
}
