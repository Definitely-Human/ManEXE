using ManExe.Interfaces;
using ManExe.Packages.QuickOutline.Scripts;
using UnityEngine;

namespace ManExe.Entity.Interactable
{
    public class OutlineSelectionResponse : MonoBehaviour, ISelectionResponse
    {


        public void OnSelect(Transform selection)
        {
            var outline = selection.GetComponent<Outline>();
            if (outline != null) outline.OutlineWidth = 3.5f;
        }

        public void OnDeselect(Transform selection)
        {
            var outline = selection.GetComponent<Outline>();
            if (outline != null) outline.OutlineWidth = 0;
        }
    }
}
