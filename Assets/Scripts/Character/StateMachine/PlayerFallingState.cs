using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    public class PlayerFallingState : PlayerBaseState, IRootState
    {
        public PlayerFallingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
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
            InitializeSubState();
            Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.IsFallingHash, false);
        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            HandleGravity();
            CheckSwitchStates(); // !Should always be at the bottom of Update State
        }

        public void HandleGravity()
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f,-20.0f);
        }
    }
}
