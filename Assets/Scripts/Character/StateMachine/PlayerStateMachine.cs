using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ManExe
{
    public class PlayerStateMachine : MonoBehaviour
    {
        // Reference variables
        private GameInput _gameInput;
        private CharacterController _characterController;
        private Animator _animator;

        // Variables to store optimized setter/getter parameter IDs
        private int _isWalkingHash;
        private int _isRunningHash;
        private int _isJumpingHash;
        private int _jumpCountHash;
        private int _isFallingHash;



        // Variables to store player input values
        private Vector2 _currentMovementInput;
        private Vector3 _currentMovement;
        private Vector3 _currentRunMovement;
        private Vector3 _appliedMovement;
        private bool _isMovementPressed;
        private bool _isRunPressed;

        // Constants
        private float _rotationFactorPerFrame = 15.0f;
        private float _runMultiplier = 3.5f; // TODO: make a property that gives speed multiplier

        // Gravity variables
        private float _gravity = -9.8f;

        // Jump varialbes
        private bool _isJumpPressed = false;
        private float _initialJumpVelocity;
        private float _maxJumpHeight = 1.0f;
        private float _maxJumpTime = 0.75f;
        private bool _isJumping = false;
        private bool _requireNewJumpPress = false;
        private int _jumpCount = 1;

        private Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
        private Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
        private Coroutine _currentJumpResetRoutine = null;

        // State variables
        PlayerBaseState _currentState;
        PlayerStateFactory _states;

        // Getters and setters
        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public Animator Animator { get => _animator; set => _animator = value; }
        public Dictionary<int, float> InitialJumpVelocities { get => _initialJumpVelocities; }
        public Dictionary<int, float> JumpGravities { get => _jumpGravities;  }
        public Coroutine CurrentJumpResetRoutine { get => _currentJumpResetRoutine; set => _currentJumpResetRoutine = value; }
        public CharacterController CharacterController { get => _characterController; set => _characterController = value; }
        public Vector2 CurrentMovementInput { get => _currentMovementInput; set => _currentMovementInput = value; }
        public bool IsJumpPressed { get => _isJumpPressed; set => _isJumpPressed = value; }
        public int JumpCount { get => _jumpCount; set => _jumpCount = value; }
        public int IsJumpingHash { get => _isJumpingHash; }
        public int IsWalkingHash { get => _isWalkingHash; }
        public int IsRunningHash { get => _isRunningHash; }
        public int JumpCountHash { get => _jumpCountHash; }
        public int IsFallingHash { get => _isFallingHash; }
        public bool IsMovementPressed { get { return _isMovementPressed; } }
        public bool IsRunPressed { get { return _isRunPressed; } }
        public bool RequireNewJumpPress { get => _requireNewJumpPress; set => _requireNewJumpPress = value; }
        public bool IsJumping { get => _isJumping; set => _isJumping = value; }
        public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
        public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
        public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
        public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
        public float Gravity { get => _gravity; }
        public float RunMultiplier { get => _runMultiplier; set => _runMultiplier = value; }



        //=====================
        // MonoBehavior methods
        //=====================
        private void Awake()
        {
            // Setup reference variables
            _gameInput = new GameInput();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            // Setup state
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterState();

            // Setup parameter hash
            _isWalkingHash = Animator.StringToHash("isWalking");
            _isRunningHash = Animator.StringToHash("isRunning");
            _isJumpingHash = Animator.StringToHash("isJumping");
            _jumpCountHash = Animator.StringToHash("jumpCount");
            _isFallingHash = Animator.StringToHash("isFalling");

            _gameInput.Player.Movement.started += OnMovementInput;
            _gameInput.Player.Movement.performed += OnMovementInput;
            _gameInput.Player.Movement.canceled += OnMovementInput;
            _gameInput.Player.Run.started += OnRun;
            _gameInput.Player.Run.canceled += OnRun;
            _gameInput.Player.Jump.started += OnJump;
            _gameInput.Player.Jump.canceled += OnJump; // TODO Put these in OnEnable and OnDisable
            SetupJumpVariables();
        }
        void Start()
        {
            _characterController.Move(_appliedMovement * Time.deltaTime);
        }

        void Update()
        {
            HandleRotation();

            _currentState.UpdateStates();
            _characterController.Move(_appliedMovement * Time.deltaTime);
        }


        private void HandleRotation()
        {
            Vector3 positionToLookAt;

            positionToLookAt.x = _currentMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = _currentMovement.z;

            Quaternion currentRotation = transform.rotation;

            if (_isMovementPressed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
            }
        }

        //========================
        // Callback functions
        private void OnRun(InputAction.CallbackContext context)
        {
            _isRunPressed = context.ReadValueAsButton();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            _isJumpPressed = context.ReadValueAsButton();
            _requireNewJumpPress = false;
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            _currentMovement.x = _currentMovementInput.x;
            _currentMovement.z = _currentMovementInput.y;

            _currentRunMovement.x = _currentMovementInput.x * _runMultiplier;
            _currentRunMovement.z = _currentMovementInput.y * _runMultiplier;
            _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
        }

        //========================
        // Methods 
        private void SetupJumpVariables()
        {
            float timeToApex = _maxJumpTime / 2;
            float initialGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
            float secondJumpGravity = (-2 * (_maxJumpHeight + 1) / Mathf.Pow((timeToApex * 1.125f), 2));
            float secondJumpInitialVelocity = (2 * (_maxJumpHeight + 1)) / (timeToApex * 1.125f);
            float thirdJumpGravity = (-2 * (_maxJumpHeight + 1.5f) / Mathf.Pow((timeToApex * 1.25f), 2));
            float thirdJumpInitialVelocity = (2 * (_maxJumpHeight + 1.5f)) / (timeToApex * 1.25f);

            _initialJumpVelocities.Add(1, _initialJumpVelocity);
            _initialJumpVelocities.Add(2, secondJumpInitialVelocity);
            _initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

            _jumpGravities.Add(0, initialGravity);
            _jumpGravities.Add(1, initialGravity);
            _jumpGravities.Add(2, secondJumpGravity);
            _jumpGravities.Add(3, thirdJumpGravity);
        }

        private void OnEnable()
        {
            _gameInput.Player.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Player.Disable();
        }
    }
}
