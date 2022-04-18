using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ManExe
{
    public class AnimationAndMovementController : MonoBehaviour
    {
        
        GameInput gameInput;
        CharacterController characterController;
        Animator animator;

        Vector2 currentMovementInput;
        Vector3 currentMovement;
        Vector3 currentRunMovement;
        Vector3 appliedMovement;
        bool isMovementPressed;
        bool isRunPressed;


        private float rotationFactorPerFrame = 15.0f;
        float runMultiplier = 3.5f; // TODO: make a property that gives speed multiplier
        int isWalkingHash;
        int isRunningHash;
        int isJumpingHash;
        int jumpCountHash;


        float groundedGravity = -.05f;
        float gravity = -9.8f;

        bool isJumpPressed = false;
        float initialJumpVelocity;
        float maxJumpHeight = 2.0f;
        float maxJumpTime = 0.75f;
        bool isJumping = false;
        bool isJumpAnimating = false;
        int jumpCount = 1;
        Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
        Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
        Coroutine currentJumpResetRoutine = null;
        void Awake()
        {
            gameInput = new GameInput();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            isWalkingHash = Animator.StringToHash("isWalking");
            isRunningHash = Animator.StringToHash("isRunning");
            isJumpingHash = Animator.StringToHash("isJumping");
            jumpCountHash = Animator.StringToHash("jumpCount");

            gameInput.Player.Movement.started += onMovementInput;
            gameInput.Player.Movement.performed += onMovementInput;
            gameInput.Player.Movement.canceled += onMovementInput;
            gameInput.Player.Run.started += onRun;
            gameInput.Player.Run.canceled += onRun;
            gameInput.Player.Jump.started += onJump;
            gameInput.Player.Jump.canceled += onJump;
            setupJumpVariables();
        }

        void setupJumpVariables()
        {
            float timeToApex = maxJumpTime / 2;
            gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
            float secondJumpGravity = (-2 * (maxJumpHeight + 2) / Mathf.Pow((timeToApex * 1.25f), 2));
            float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
            float thirdJumpGravity = (-2 * (maxJumpHeight + 4) / Mathf.Pow((timeToApex * 1.5f), 2));
            float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

            initialJumpVelocities.Add(1, initialJumpVelocity);
            initialJumpVelocities.Add(2, secondJumpInitialVelocity);
            initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

            jumpGravities.Add(0, gravity);
            jumpGravities.Add(1, gravity);
            jumpGravities.Add(2, secondJumpGravity);
            jumpGravities.Add(3, thirdJumpGravity);
        }

        void handleJump()
        {
            if(!isJumping && characterController.isGrounded && isJumpPressed)
            {
                if(jumpCount < 3 && currentJumpResetRoutine != null)
                {
                    StopCoroutine(currentJumpResetRoutine);
                }
                animator.SetBool(isJumpingHash, true); // TODO: Make this event based
                isJumpAnimating = true;
                isJumping = true;
                currentMovement.y = initialJumpVelocities[jumpCount];
                appliedMovement.y = initialJumpVelocities[jumpCount] ;
                Debug.Log(jumpCount+ " " + animator.GetInteger(jumpCountHash));
                animator.SetInteger(jumpCountHash, jumpCount);// TODO: Put this into jumpCount SETTER
                if (jumpCount < 3 && jumpCount >=1)
                    jumpCount++;
                else
                    jumpCount = 1;
                
            }
            else if(!isJumpPressed && isJumping && characterController.isGrounded)
            {
                isJumping = false;
            }
        }

        IEnumerator jumpResetRoutine()
        {
            yield return new WaitForSeconds(.5f);
            jumpCount = 1;
            animator.SetInteger(jumpCountHash, jumpCount);
        }

        private void onRun(InputAction.CallbackContext context)
        {
            isRunPressed = context.ReadValueAsButton();
        }

        private void onJump(InputAction.CallbackContext context)
        {
            isJumpPressed = context.ReadValueAsButton();
        }

        void handleRotation()
        {
            Vector3 positionToLookAt;

            positionToLookAt.x = currentMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = currentMovement.z;

            Quaternion currentRotation = transform.rotation;

            if (isMovementPressed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation =  Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
            }
        }

        void onMovementInput(InputAction.CallbackContext context)
        {
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;

            currentRunMovement.x = currentMovementInput.x * runMultiplier;
            currentRunMovement.z = currentMovementInput.y * runMultiplier;
            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        }

        void handleAnimation()
        {
            bool isWalking = animator.GetBool(isWalkingHash);
            bool isRunning = animator.GetBool(isRunningHash);

            if(isMovementPressed && !isWalking)
            {
                animator.SetBool(isWalkingHash, true);
            }
            else if (!isMovementPressed && isWalking)
            {
                animator.SetBool(isWalkingHash, false);
            }

            if((isMovementPressed && isRunPressed) && !isRunning)
            {
                animator.SetBool(isRunningHash, true);
            }
            else if ((!isMovementPressed || !isRunPressed) && isRunning)
            {
                animator.SetBool(isRunningHash, false);
            }
        }

        void handleGravity()
        {
            bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed; // TODO Optimize isFalling condition
            float fallMultiplier = 2.0f;
            if (characterController.isGrounded)
            {
                if (isJumpAnimating)
                {
                    animator.SetBool(isJumpingHash, false);
                    isJumpAnimating = false;
                    currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                }
                animator.SetBool(isJumpingHash, false);
                currentMovement.y = groundedGravity;
                appliedMovement.y = groundedGravity;
            }
            else if (isFalling)
            {
                float previousYVelocity = currentMovement.y;
                currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
                appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f,-20.0f);
            }
            else
            {
                float previousYVelocity = currentMovement.y;
                currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
                appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
            }
        }

        void Update()
        {
            handleAnimation();
            handleRotation();
            if (isRunPressed)
            {
                appliedMovement.x = currentRunMovement.x;
                appliedMovement.z = currentRunMovement.z;
            }
            else
            {
                appliedMovement.x = currentMovement.x;
                appliedMovement.z = currentMovement.z;
                
            }
            characterController.Move(appliedMovement * Time.deltaTime);
            handleGravity();
            handleJump();
        }

        private void OnEnable()
        {
            gameInput.Player.Enable();
        }

        private void OnDisable()
        {
            gameInput.Player.Disable();
        }
    }
}
