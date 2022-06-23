using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
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
