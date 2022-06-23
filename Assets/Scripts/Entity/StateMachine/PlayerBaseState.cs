
namespace ManExe
{

    public abstract class PlayerBaseState 
    {
        private bool _isRootState = false;
        private PlayerStateMachine _ctx;
        private PlayerStateFactory _factory;
        private PlayerBaseState _currentSuperState;
        private PlayerBaseState _currentSubState;

        protected bool IsRootState { get => _isRootState; set => _isRootState = value; }
        protected PlayerStateMachine Ctx { get => _ctx; set => _ctx = value; }
        protected PlayerStateFactory Factory { get => _factory; set => _factory = value; }

        protected PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory){
            _ctx = currentContext;
            _factory = playerStateFactory;
    	}

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();

        public void UpdateStates() {
            UpdateState();
            if(_currentSubState != null)
            {
                _currentSubState.UpdateStates();
            }
        }
        protected void SwitchState( PlayerBaseState newState) {
            ExitState();
            newState.EnterState();
            if (_isRootState)
                _ctx.CurrentState = newState;
            else if (_currentSuperState != null)
                _currentSuperState.SetSubState(newState);
        }
        protected void SetSuperState(PlayerBaseState newSuperState) {
            _currentSuperState = newSuperState;
        }
        protected void SetSubState(PlayerBaseState newSubState) {
            _currentSubState = newSubState;
            _currentSubState.EnterState();
            newSubState.SetSuperState(this);
        }
    
    }
}
