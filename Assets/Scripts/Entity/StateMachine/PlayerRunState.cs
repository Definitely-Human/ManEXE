using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManExe
{

    public class PlayerRunState : PlayerBaseState
    {
    	public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory): base(currentContext,playerStateFactory)
        {

            IsRootState = false;
        }

        public override void CheckSwitchStates()
        {
            if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            {
                SwitchState(Factory.Walk());
            }
        }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
            Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            Ctx.AppliedMovementX = Ctx.CurrentMovementX * Ctx.RunMultiplier;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementZ * Ctx.RunMultiplier;
            CheckSwitchStates(); // !Should always be at the bottom of Update State
        }
    }
}
