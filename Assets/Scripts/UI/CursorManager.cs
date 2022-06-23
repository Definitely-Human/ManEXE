using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{
    public class CursorManager : MonoBehaviour
    {
        
        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
