using StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContex : ReactiveContext<EnemyContex>
{
    public int maxHealth;
    public int baseHealth;
    private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        set => SetProperty(ref currentHealth, value, nameof (CurrentHealth));
    }
    public NavMeshAgent agent;
}
