using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Infrastructure.GameStates.Interfaces;

namespace _Project.Code.Infrastructure
{
    public class GameStateMachine
    {
        private IExitableGameState _currentState;
        private readonly Dictionary<Type, IExitableGameState> _states;

        public GameStateMachine(List<IExitableGameState> states)
        {
            _states = states.ToDictionary(x => x.GetType(), x => x);
        }
        
        public TState Enter<TState>() where TState : class, IGameState
        {
            IGameState state = ChangeState<TState>();
            state.Enter();
            return (TState)state;
        }

        public TState Enter<TState, TPayload>(TPayload payload) where TState : 
            class, IGameStateWithPayload<TPayload>
        {
            IGameStateWithPayload<TPayload> state = ChangeState<TState>();
            state.Enter(payload);
            return (TState)state;
        }

        private TState ChangeState<TState>() where TState : class, IExitableGameState
        {
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableGameState
        {
            return _states[typeof(TState)] as TState;
        }
    }
    
}