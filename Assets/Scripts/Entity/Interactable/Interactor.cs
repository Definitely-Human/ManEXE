using ManExe.Interfaces;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.Entity.Interactable
{
    public class Interactor : MonoBehaviour
    {
        public Transform InteractionPoint;
        public LayerMask InteractionLayer;
        public float InteractionPointRadius = 1f;
        private InputReader _inputReader;
        public bool IsInteracting { get; private set; }
        
        private void Awake()
        {
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");
        }
        
        private void OnEnable()
        {
            _inputReader.InterractEvent += OnInteraction;
        }
        
        
        private void OnDisable()
        {
            _inputReader.InterractEvent -= OnInteraction;
        }
        
        private void OnInteraction()
        {
            var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);


            for (var i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null) StartInteraction(interactable);
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