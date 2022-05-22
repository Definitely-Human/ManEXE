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
        private Transform _cameraMainTransform;
        private Transform _cameraAngleReference;
        private World _world;

        // Variables to store optimized setter/getter parameter IDs
        private int _isWalkingHash;
        private int _isRunningHash;
        private int _isJumpingHash;
        private int _jumpCountHash;
        private int _isFallingHash;

        // Variables to store player input values
        private Vector2 _currentMovementInput;
        private Vector3 _currentMovement;
        private Vector3 _appliedMovement;
        private bool _isMovementPressed;
        private bool _isRunPressed;
        private bool _requreBreakBlock;
        private bool _requrePlaceBlock;

        // Constants
        private float _rotationFactorPerFrame = 15.0f;
        private float _runMultiplier = 3.5f;
        private int _layerMask;

        // Gravity variables
        private float _gravity = -9.8f;

        // Jump varialbes
        private bool _isJumpPressed = false;
        private float _initialJumpVelocity;
        private float _maxJumpHeight = 1.0f;
        private float _maxJumpTime = 0.75f;
        private float _fallSpeedLimit = -20.0f;
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
        public float CurrentMovementX { get { return _currentMovement.x; } set { _currentMovement.x = value; } }
        public float CurrentMovementZ { get { return _currentMovement.z; } set { _currentMovement.z = value; } }
        public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
        public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
        public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
        public float Gravity { get => _gravity; }
        public float RunMultiplier { get => _runMultiplier; set => _runMultiplier = value; }
        public float FallSpeedLimit { get => _fallSpeedLimit; }

        //=====================
        // MonoBehavior methods
        //=====================
        private void Awake()
        {
            // Setup reference variables
            _gameInput = new GameInput();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _cameraMainTransform = Camera.main.transform;
            _world = GameObject.Find("World").GetComponent<World>();

            _cameraAngleReference = new GameObject().transform;
            _cameraAngleReference.name = "Camera Angle Reference";
            _cameraAngleReference.SetParent(transform.parent);

            // Setup parameter hash
            _isWalkingHash = Animator.StringToHash("isWalking");
            _isRunningHash = Animator.StringToHash("isRunning");
            _isJumpingHash = Animator.StringToHash("isJumping");
            _jumpCountHash = Animator.StringToHash("jumpCount");
            _isFallingHash = Animator.StringToHash("isFalling");

            // Setup state
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterState();


            _layerMask = 1 << 6;

            SetupJumpVariables();
        }
        void Start()
        {
            _characterController.Move(_appliedMovement * Time.deltaTime);
        }

        void Update()
        {
            HandleRotation();
            AdjustMovmentFromCameraAngle();


            HandlePlacementAndDestruction();

            _currentState.UpdateStates();

            _characterController.Move(_appliedMovement * Time.deltaTime);

            
        }

        private void HandlePlacementAndDestruction()
        {
            if (_requrePlaceBlock)
            {
                Ray ray = _cameraMainTransform.gameObject.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10, _layerMask))
                {
                    Debug.Log("Ray hit " + hit.ToString());
                        _world.GetChunkFromVector3(hit.transform.position).PlaceTerrain(hit.point);
                    

                }
                _requrePlaceBlock = false;
            }

            if (_requreBreakBlock)
            {

                Ray ray = _cameraMainTransform.gameObject.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit,10, _layerMask))
                {
                    _world.GetChunkFromVector3(hit.transform.position).RemoveTerrain(hit.point);
                    

                }
                _requreBreakBlock = false;
            }
        }

        private void OnEnable()
        {
            _gameInput.Player.Movement.started += OnMovementInput;
            _gameInput.Player.Movement.performed += OnMovementInput;
            _gameInput.Player.Movement.canceled += OnMovementInput;
            _gameInput.Player.Run.started += OnRun;
            _gameInput.Player.Run.canceled += OnRun;
            _gameInput.Player.Jump.started += OnJump;
            _gameInput.Player.Jump.canceled += OnJump;
            _gameInput.Player.MouseLeft.started += OnLeftMouseClick;
            _gameInput.Player.MouseRight.started += OnRightMouseClick;

            _gameInput.Player.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Player.Movement.started -= OnMovementInput;
            _gameInput.Player.Movement.performed -= OnMovementInput;
            _gameInput.Player.Movement.canceled -= OnMovementInput;
            _gameInput.Player.Run.started -= OnRun;
            _gameInput.Player.Run.canceled -= OnRun;
            _gameInput.Player.Jump.started -= OnJump;
            _gameInput.Player.Jump.canceled -= OnJump;
            _gameInput.Player.MouseLeft.started -= OnLeftMouseClick;
            _gameInput.Player.MouseRight.started -= OnRightMouseClick;

            _gameInput.Player.Disable();
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

        private void OnLeftMouseClick(InputAction.CallbackContext context)
        {
            _requreBreakBlock = true;
        }

        private void OnRightMouseClick(InputAction.CallbackContext context)
        {
            _requrePlaceBlock = true;
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            //_currentMovement.x = _currentMovementInput.x;
            //_currentMovement.z = _currentMovementInput.y;
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

        private void AdjustMovmentFromCameraAngle()
        {
            _cameraAngleReference.eulerAngles = new Vector3(0, _cameraMainTransform.eulerAngles.y, 0); 
            //Instead of taking the camera's forward Vector, use a reference Transform,
            //which will take the camera's y euler axis as a reference, but not the x. So when camera looks down or up,
            //it won't be affected by that, and it's forward vector will be just like camera is looking forward.

            Vector3 cameraAdjustedMovement = _cameraAngleReference.forward * _currentMovementInput.y + _cameraAngleReference.right * _currentMovementInput.x;
            _currentMovement.x = cameraAdjustedMovement.x;
            _currentMovement.z = cameraAdjustedMovement.z;
        }

        private void HandleRotation()
        {
            Vector3 positionToLookAt;

            positionToLookAt.x = _currentMovement.x;
            positionToLookAt.z = _currentMovement.z;
            positionToLookAt.y = 0.0f;
            Quaternion currentRotation = transform.rotation;

            if (_isMovementPressed)
            {
                if (positionToLookAt != Vector3.zero)// Error message pops when you try to call a rotation method towards a vector zero (0,0,0) 
                {
                    Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

                    transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
                }
            }
        }
    }
}
