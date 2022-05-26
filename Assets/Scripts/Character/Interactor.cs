using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ManExe
{

    public class Interactor : MonoBehaviour
    {
        public Transform InteractionPoint;
        public LayerMask InteractionLayer;
        public float InteractionPointRadius = 1f;
        public bool IsInteracting { get; private set; }

        private void Update()
        {
            var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    var interactable = colliders[i].GetComponent<IInteractable>();

                    if (interactable != null) StartInteraction(interactable);
                }
            }
        }

        private void StartInteraction(IInteractable interactable)
        {
            interactable.Interact(this, out bool interactSuccessful);
            IsInteracting = true;
        }

        private void EndInteraction()
        {
            IsInteracting = false;
        }
    }
}
