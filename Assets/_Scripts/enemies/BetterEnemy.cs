using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class BetterEnemy : MonoBehaviour
{
   public enum EnemyState
    {
        chase, Patrol
    }

    public Transform player;
    public EnemyState currentState;

    Transform target;
    NavMeshAgent agent;

    public Transform[] path;
    public int pathIndex = 1;
    public float distThreshold = 0.2f; // floating point math is inexact, this allows us to get close enough to the waypoint and move to the next one.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!player) return;

        if (currentState == EnemyState.chase) target = player;

        if (currentState == EnemyState.Patrol)
        {
            if (target == player) target = path[pathIndex];

            if(agent.remainingDistance < distThreshold)
            {
                pathIndex++;
                //0 mod 4 - return 0 - 4 mod 4 return 0
                pathIndex %= path.Length;
                target = path[pathIndex];
                //if we reach the end of the path - go back to zero
                //if (pathIndex == path.Length) pathIndex = 0;
            }
        }
        agent.SetDestination(target.position);
    }
}
