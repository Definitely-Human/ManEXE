using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class RayCastBasedTagSelector : MonoBehaviour, ISelector
    {
        [SerializeField] private string _selectableTag = "Selectable";
        [SerializeField] private LayerMask _layerMask ;
        [SerializeField] private int _maxDistance;
        private Transform _selection;
        
        public void Check(Ray ray)
        {
            _selection = null;
            if (Physics.Raycast(ray, out var hit,_maxDistance,_layerMask))
            {
                var selection = hit.transform;
                if (selection.CompareTag(_selectableTag))
                {
                    _selection = selection;
                }
            }
        }

        public Transform GetSelection()
        {
            return this._selection;
        }
    }
}
