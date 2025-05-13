using UnityEngine;
using StateMachine;
using StateMachine.Editor;
using UnityEngine.AI;
public class EnemyStateMachine : StateMachine<EnemyContex>
{
    public IState<EnemyContex> idelState;
    public IState<EnemyContex> patrolState;
    public IState<EnemyContex> chaseState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        context = new EnemyContex()
        {
            baseHealth = 50,
            CurrentHealth = 50,
            maxHealth = 100,
            agent = GetComponent<NavMeshAgent>()
        };

        StateBuilder<EnemyContex> stateBuilder = new StateBuilder<EnemyContex>();

        idelState = stateBuilder
            .SetName("idle")
            .OnEnter(() => Debug.Log("Entering Idle State"))
            .Build();
        patrolState = stateBuilder
            .SetName("patrol")
            .OnEnter(() => Debug.Log("Entering patrol State"))
            .Build();

        chaseState = stateBuilder
            .SetName("chase")
            .OnEnter(() => Debug.Log("Entering chase State"))
            .Build();

        RegisterState("idle", idelState);
        RegisterState("partol", patrolState);
        RegisterState("chase", chaseState);

        idelState.AddTransition(this)
            .To(patrolState)
            .When(idelToPatrolFunction)
            .WithAction(() => Debug.Log("Action for idel to patrol"))
            .WithPriority(10);


        StateMachineRegistry.RegisterStateMachine(this, "EnemyStateMachine");
    }

    bool idelToPatrolFunction()
    {
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
