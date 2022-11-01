using UnityEngine;

namespace ManExe.Interfaces
{
    public interface ISelectionResponse
    {
        void OnSelect(Transform selection);
        void OnDeselect(Transform selection);

    }
}
