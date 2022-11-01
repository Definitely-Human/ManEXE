using UnityEngine;

namespace ManExe.Interfaces
{

    public interface ISelector
    {
        void Check(Ray ray);

        Transform GetSelection();

    }
}
