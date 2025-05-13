using System;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Interface for state machine states.
    /// </summary>
    /// <typeparam name="TContext">The type of context data used by the state</typeparam>
    public interface IState<TContext> where TContext : class
    {
        /// <summary>
        /// Called when entering the state.
        /// </summary>
        void Enter();
        
        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        void Exit();
        
        /// <summary>
        /// Called during the update loop.
        /// </summary>
        void Update();
        
        /// <summary>
        /// Called during the fixed update loop.
        /// </summary>
        void FixedUpdate();

        void Init(StateMachine<TContext> stateMachine, TContext context);
    }
}
