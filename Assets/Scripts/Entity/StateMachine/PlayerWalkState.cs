namespace ManExe.Entity.StateMachine
{

    public class PlayerWalkState : PlayerBaseState
    {
    	public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext,playerStateFactory)
        {

            IsRootState = false;
        }

        public override void CheckSwitchStates()
        {
            if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
            {
                SwitchState(Factory.Run());
            }
        }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
            Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            Ctx.AppliedMovementX = Ctx.CurrentMovementX;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementZ;
            CheckSwitchStates(); // !Should always be at the bottom of Update State
        }
    }
}
