using System;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Base class for states that need access to the context and state machine.
    /// </summary>
    /// <typeparam name="TContext">The type of context data used by the state</typeparam>
    public abstract class StateBase<TContext, TStateMachine> : IState<TContext> 
        where TContext : class
        where TStateMachine : StateMachine<TContext>
    {
        // References to the state machine and context
        protected TStateMachine stateMachine;
        protected TContext context;

        /// <summary>
        /// Sets up transitions between states. This method can be overridden in derived classes to set up transitions between states. 
        /// By default, it does nothing.
        /// </summary>
        public virtual void SetupTransitions() { }

        /// <summary>
        /// Called when entering the state.
        /// </summary>
        public virtual void Enter() { }
        
        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        public virtual void Exit() { }
        
        /// <summary>
        /// Called during the update loop.
        /// </summary>
        public virtual void Update() { }
        
        /// <summary>
        /// Called during the fixed update loop.
        /// </summary>
        public virtual void FixedUpdate() { }

        public void Init(StateMachine<TContext> stateMachine, TContext context)
        {
            this.stateMachine = stateMachine as TStateMachine;
            this.context = context;
        }
    }
}
