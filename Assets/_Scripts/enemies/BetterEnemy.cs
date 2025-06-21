using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class BetterEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        chase, Patrol
    }

    public bool Chase = false;
    public bool Patrol = false;


    public int curHealth;
    public int maxHealth;
    public int baseHealth;
    public event Action<int> OnHealthChanged;

    public GameObject powerUp;
    [SerializeField] private Vector3 spawnOffSet = new Vector3(0, 50, 0);
    //attack varibales
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public Transform player;
    public EnemyState currentState;

    AudioSource audioSource;
    Transform target;
    NavMeshAgent agent;
    Animator anim;
   
    public Transform[] path;
    public int pathIndex = 1;
    public float distThreshold = 0.2f; // floating point math is inexact, this allows us to get close enough to the waypoint and move to the next one.

    //enemy audio
    public AudioClip DeathClip;
    public AudioClip HitClip;

    public float elapsedTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        baseHealth = 5;
        maxHealth = 10;
        curHealth = 5;

        target = path.Length > 0 ? path[0] : player ? player.transform : null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target) return;
        //chase
        if (currentState == EnemyState.chase && curHealth > 0)
        {
            Chase = true;
            Patrol = false;

            anim.SetBool("Chase", true);
            anim.SetBool("Patrol", false);
            target = player;

            float disToPlayer = Vector3.Distance(transform.position, player.position);

            if (disToPlayer <= attackRange)
            {
                agent.isStopped = true;

                anim.SetTrigger("Attack");

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
        }
        //patrol
        if (currentState == EnemyState.Patrol && curHealth > 0)
            {
                Patrol = true;
                Chase = false;

                anim.SetBool("Patrol", true);
                anim.SetBool("Chase", false);
                agent.isStopped = false;

            if (target == player) target = path[pathIndex];

                if (agent.remainingDistance < distThreshold)
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
            _player.hasHit = true;
            DamageTaken();
        }
        if(other.gameObject.CompareTag("player"))
        {
            
            GameManager.Instance.Lives--;

            _player.PlayHitSound();

            if (GameManager.Instance.Lives <= 0)
            {
                _player.PlayDeathSound();
            }
            
            //this is using the player object Animator
            Animator playeranim = _player.GetComponentInChildren<Animator>();
            if (playeranim != null)
            {
                Debug.Log("Player Hit");
                playeranim.SetTrigger("Hit");
            }
        }
    }

    public void DamageTaken()
    {
        audioSource.PlayOneShot(HitClip);
        curHealth -= 1;
        anim.SetTrigger("Hit");


        if (curHealth <= 0)
        {
            audioSource.PlayOneShot(DeathClip);

            agent.isStopped = true;
            anim.SetTrigger("Die");
            Destroy(gameObject, 3f);

            Vector3 spawnPos = new Vector3(transform.position.x, 50f, transform.position.z);
            Instantiate(powerUp, spawnPos, transform.rotation);
            Debug.Log($"PowerUp Moved {spawnPos}");
        }

        OnHealthChanged?.Invoke(curHealth);
    }
}
