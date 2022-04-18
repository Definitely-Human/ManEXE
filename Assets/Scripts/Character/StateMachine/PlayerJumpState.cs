using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    public class PlayerJumpState : PlayerBaseState, IRootState
    {
        public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            InitializeSubState();
            IsRootState = true;
        }

        public override void CheckSwitchStates()
        {
            if (Ctx.CharacterController.isGrounded)
            {
                SwitchState(Factory.Grounded());
            }
        }

        public override void EnterState()
        {
            HandleJump();
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
            if(Ctx.IsJumpPressed)
                Ctx.RequireNewJumpPress = true;
            Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(IJumpResetRoutine());
        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            HandleGravity();
            CheckSwitchStates(); // !Should always be at the bottom of Update State
            
        }

        private void HandleJump()
        {

            if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetRoutine != null)
            {
                Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
            }
            Ctx.Animator.SetBool(Ctx.IsJumpingHash, true); // TODO: Make this event based
            Ctx.IsJumping = true;
            Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
            Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);// TODO: Put this into jumpCount SETTER
            if (Ctx.JumpCount < 3 && Ctx.JumpCount >= 1)
                Ctx.JumpCount++;
            else
                Ctx.JumpCount = 1;


        }

        public void HandleGravity()
        {
            bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed; // TODO Optimize isFalling condition
            float fallMultiplier = 2.0f;
            if (isFalling)
            {
                float previousYVelocity = Ctx.CurrentMovementY;
                Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
                Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
            }
            else
            {
                float previousYVelocity = Ctx.CurrentMovementY;
                Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
                Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * .5f;
            }
        }

        private IEnumerator IJumpResetRoutine()
        {
            yield return new WaitForSeconds(.5f);
            Ctx.JumpCount = 1;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        }
    }
}
