using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework; 

namespace PokemonGame.Engine
{
    /// A generic, reusable Finite State Machine (FSM) for C#.
    public class StateMachine
    {
        private IState _currentState;
        private readonly object _owner; // The object this FSM belongs to (e.g., a Pokemon instance)
        private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();

        public string CurrentStateName { get; private set; }

        public StateMachine(object owner)
        {
            _owner = owner;
        }
        
        /// Adds a state to the machine's dictionary.
        public void AddState(string name, IState state)
        {
            _states[name] = state;
        }

        /// Transitions to a new state.
        public void TransitionTo(string newStateName, params object[] args)
        {
            if (!_states.ContainsKey(newStateName))
            {
                Console.WriteLine($"Error: State '{newStateName}' not found.");
                return;
            }

            // 1. Call the exit method of the current state
            _currentState?.Exit();

            // 2. Update to the new state
            _currentState = _states[newStateName];
            CurrentStateName = newStateName;

            // 3. Call the enter method of the new state
            _currentState.Enter(_owner, args);
        }

        /// Calls the Update method of the current state.
        public void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
        }
    }
}
