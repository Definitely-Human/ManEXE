using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ManExe
{
    [CreateAssetMenu(fileName = "New InputReader", menuName = "Scriptable/InputReader", order = 0)]
    public class InputReader : ScriptableObject, GameInput.ICheatsActions, GameInput.IPlayerActions, GameInput.IMenuActions
    {
        //=============================
        //=== Actions
        //=============================

        // Player
        public event UnityAction JumpEvent = delegate { };

        public event UnityAction JumpCanceledEvent = delegate { };

        public event UnityAction AttackEvent = delegate { };

        public event UnityAction AttackCanceledEvent = delegate { };

        public event UnityAction UseEvent = delegate { }; // Right mouse button (place block, etc.)

        public event UnityAction InterractEvent = delegate { };

        public event UnityAction PauseEvent = delegate { };

        public event UnityAction OpenInventoryEvent = delegate { };

        public event UnityAction RunEvent = delegate { };

        public event UnityAction RunStoppedEvent = delegate { };

        public event UnityAction<Vector2, bool> CameraEvent = delegate { };

        public event UnityAction<Vector2> MovementEvent = delegate { };

        // Menu
        public event UnityAction MoveSelectionEvent = delegate { };

        public event UnityAction ConfirmEvent = delegate { };

        public event UnityAction CancelEvent = delegate { }; // Close menu, or uncofirm

        public event UnityAction UnpauseEvent = delegate { };

        //Cheats
        public event UnityAction DebugMenuEvent = delegate { };

        public event UnityAction ConsoleEvent = delegate { };

        public event UnityAction SaveLoadMenuEvent = delegate { };

        // Private fields
        private GameInput _gameInput;

        //=============================
        //=== Scriptable Object Methods
        //=============================
        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();

                _gameInput.Menu.SetCallbacks(this);
                _gameInput.Player.SetCallbacks(this);
                _gameInput.Cheats.SetCallbacks(this);
            }
            _gameInput.Cheats.Enable(); // Temporary solution
            _gameInput.Player.Enable();
            _gameInput.Menu.Enable();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        //=============================
        //=== Methods
        //=============================

        // Switch action maps
        private void EnablePlayerInput()
        {
            _gameInput.Player.Enable();
            _gameInput.Menu.Disable();
        }

        private void EnableMenuInput()
        {
            _gameInput.Player.Disable();
            _gameInput.Menu.Enable();
        }

        private void DisableAllInput()
        {
            _gameInput.Cheats.Disable();
            _gameInput.Player.Disable();
            _gameInput.Menu.Disable();
        }

        // Player Events

        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

        public void OnAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    AttackEvent?.Invoke();
                    break;

                case InputActionPhase.Canceled:
                    AttackCanceledEvent?.Invoke();
                    break;
            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            UseEvent?.Invoke();
        }

        public void OnCamera(InputAction.CallbackContext context)
        {
            CameraEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnInteract(InputAction.CallbackContext context)
        {
            InterractEvent?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                JumpEvent?.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent?.Invoke();
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            OpenInventoryEvent?.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            PauseEvent?.Invoke();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    RunEvent?.Invoke();
                    break;

                case InputActionPhase.Canceled:
                    RunStoppedEvent?.Invoke();
                    break;
            }
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementEvent?.Invoke(context.ReadValue<Vector2>());
        }

        // Menu Events

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                CancelEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            ConfirmEvent?.Invoke();
        }

        public void OnMoveSelection(InputAction.CallbackContext context)
        {
            MoveSelectionEvent?.Invoke();
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            UnpauseEvent?.Invoke();
        }

        // Cheat Events

        public void OnConsole(InputAction.CallbackContext context)
        {
            ConsoleEvent?.Invoke();
        }

        public void OnDebugMenu(InputAction.CallbackContext context)
        {
            DebugMenuEvent?.Invoke();
        }

        public void OnSaveLoadMenu(InputAction.CallbackContext context)
        {
            SaveLoadMenuEvent?.Invoke();
        }
    }
}