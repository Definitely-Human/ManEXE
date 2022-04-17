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
        bool isMovementPressed;
        bool isRunPressed;
        private float rotationFactorPerFrame = 15.0f;
        float runMultiplier = 2.0f; // TODO: make a property that gives speed multiplier
        int isWalkingHash = Animator.StringToHash("isWalking");
        int isRunningHash = Animator.StringToHash("isRunning");

        void Awake()
        {
            gameInput = new GameInput();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            gameInput.Player.Movement.started += onMovementInput;
            gameInput.Player.Movement.performed += onMovementInput;
            gameInput.Player.Movement.canceled += onMovementInput;
            gameInput.Player.Run.started += onRun;
            gameInput.Player.Run.canceled += onRun;
        }

        private void onRun(InputAction.CallbackContext context)
        {
            isRunPressed = context.ReadValueAsButton();
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
            if (characterController.isGrounded)
            {

            }
        }

        void Update()
        {
            handleAnimation();
            handleRotation();

            if (isRunPressed)
            {
                characterController.Move(currentRunMovement * Time.deltaTime);
            }
            else
            {
                characterController.Move(currentMovement * Time.deltaTime);
            }
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
