using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    public class PlayerGroundedState : PlayerBaseState, IRootState
    {
    	public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
    	}

        public override void CheckSwitchStates()
        {
            if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
            {
                SwitchState(Factory.Jump());
            }
            else if (!Ctx.CharacterController.isGrounded)
            {
                SwitchState(Factory.Falling());
            }
        }

        public override void EnterState()
        {
            InitializeSubState();
        }

        public override void ExitState()
        {

        }

        public void HandleGravity()
        {
            Ctx.CurrentMovementY = Ctx.Gravity;
            Ctx.AppliedMovementY = Ctx.Gravity;
        }

        public override void InitializeSubState()
        {
            if(!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            {
                SetSubState(Factory.Idle());
            }
            else if(Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            {
                SetSubState(Factory.Walk());
            }
            else
            {
                SetSubState(Factory.Run());
            }
        }

        public override void UpdateState()
        {
            HandleGravity();
            CheckSwitchStates(); // !Should always be at the bottom of Update State
        }
    }
}
