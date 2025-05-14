using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class BetterEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        chase, Patrol
    }

    public int curHealth;
    public int maxHealth;
    public int baseHealth;

    public GameObject powerUp;
    [SerializeField] private Vector3 spawnOffSet = new Vector3(0, 5, 0);

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

        baseHealth = 5;
        maxHealth = 10;
        curHealth = 5;

        target = path.Length > 0 ? path[0] : player ? player.transform : null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target) return;

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

    public Player _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("weapon") && _player.attack && !_player.hasHit)
        {
            DamageTaken();
            _player.hasHit = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("weapon") && _player.attack && !_player.hasHit)
        {
            DamageTaken();
            _player.hasHit = true;
        }
    }




    private void DamageTaken()
    {
        curHealth -= 1;

        if (curHealth <= 0)
        {
            Destroy(gameObject);
            
            Instantiate(powerUp, transform.position + spawnOffSet, transform.rotation);
        }
    }
}
