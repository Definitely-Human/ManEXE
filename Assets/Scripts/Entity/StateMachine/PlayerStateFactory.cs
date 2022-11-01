using System.Collections.Generic;

namespace ManExe.Entity.StateMachine
{
    enum PlayerStates
    {
        idle,
        walk,
        run,
        grounded,
        jump,
        falling
    }

    public class PlayerStateFactory 
    {
        PlayerStateMachine _context;
        Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();
    	public PlayerStateFactory(PlayerStateMachine currentContext){
            _context = currentContext;
            _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
            _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
            _states[PlayerStates.run] = new PlayerRunState(_context, this);
            _states[PlayerStates.jump] = new PlayerJumpState(_context, this);
            _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
            _states[PlayerStates.falling] = new PlayerFallingState(_context, this);
    	}

        public PlayerBaseState Idle()
        {
            return _states[PlayerStates.idle];
        }
        public PlayerBaseState Walk()
        {
            return _states[PlayerStates.walk];
        }
        public PlayerBaseState Run()
        {
            return _states[PlayerStates.run];
        }
        public PlayerBaseState Jump()
        {
            return _states[PlayerStates.jump];
        }
        public PlayerBaseState Grounded()
        {
            return _states[PlayerStates.grounded];
        }
        public PlayerBaseState Falling()
        {
            return _states[PlayerStates.falling];
        }
    }
}
