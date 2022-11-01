using ManExe.Entity.Inventory;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.UI.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        public DynamicInventoryDisplay externalInventoryPanel;
        public DynamicInventoryDisplay playerBackpackPanel;
        private InputReader _inputReader;
        [SerializeField] private CursorManager _cursorManager;

        public bool IsInventoryOpened
        {
            get => externalInventoryPanel.gameObject.activeInHierarchy ||
                   playerBackpackPanel.gameObject.activeInHierarchy;
        }

        private void Awake()
        {
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");
            _cursorManager.LockCursor();
        }

        private void Start()
        {
            externalInventoryPanel.gameObject.SetActive(false);
            playerBackpackPanel.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayExternalInventory;
            PlayerInventroryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
            _inputReader.PauseEvent += CloseExternalInventory;
            _inputReader.PauseEvent += CloseBackpackInventory;
        }

        

        private void OnDisable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayExternalInventory;
            PlayerInventroryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
            _inputReader.PauseEvent -= CloseExternalInventory;
            _inputReader.PauseEvent -= CloseBackpackInventory;
        }
        
        private void CloseBackpackInventory()
        {
            if (playerBackpackPanel.gameObject.activeInHierarchy)
            {
                playerBackpackPanel.gameObject.SetActive(false);
                if (!IsInventoryOpened) // If both inventories are closed lock cursor
                {
                    _cursorManager.LockCursor();
                }
            }

            
        }

        private void CloseExternalInventory()
        {
            if (externalInventoryPanel.gameObject.activeInHierarchy)
            {
                externalInventoryPanel.gameObject.SetActive(false);
                if (!IsInventoryOpened)
                {
                    _cursorManager.LockCursor();
                }
            }
            
            
        }

        private void DisplayExternalInventory(InventorySystem invToDisplay, int offset)
        {
            externalInventoryPanel.gameObject.SetActive(true);
            externalInventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
            _cursorManager.UnlockCursor();
            
        }
        private void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
        {
            playerBackpackPanel.gameObject.SetActive(true);
            playerBackpackPanel.RefreshDynamicInventory(invToDisplay, offset);
            _cursorManager.UnlockCursor();
        }

    }
}
