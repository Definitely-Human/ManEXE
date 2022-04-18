using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    public class PlayerIdleState : PlayerBaseState
    {
    	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {

            IsRootState = false;
        }

        public override void CheckSwitchStates()
        {
            if(Ctx.IsMovementPressed && Ctx.IsRunPressed)
            {
                SwitchState(Factory.Run());
            }
            else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            {
                SwitchState(Factory.Walk());
            }
        }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
            Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
            Ctx.AppliedMovementX = 0;
            Ctx.AppliedMovementZ = 0;
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            CheckSwitchStates(); // !Should always be at the bottom of Update State
        }
    }
}
