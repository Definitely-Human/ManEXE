using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    public interface ISelector
    {
        void Check(Ray ray);

        Transform GetSelection();

    }
}
