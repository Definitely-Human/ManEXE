using ManExe.Entity.Interactable;
using UnityEngine.Events;

namespace ManExe.Interfaces
{

    public interface IInteractable 
    {
    	public UnityAction<IInteractable> OnInteractionComplete { get; set; }
        public void Interact(Interactor interactor, out bool interactSuccessful);

        public void EndInteraction();
    
    }
}
