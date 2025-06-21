using UnityEngine;
using UnityEngine.AI;

public class ShooterEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        shoot, Patrol
    }

    public EnemyState currentState;
    public Transform player; // Target to follow
    public Transform BulletSpawnPoint; // Where bullets are spawned

    public GameObject bulletPrefab; // Bullet prefab to instantiate
    public float shootRange = 10f; // Range within which the enemy can shoot
    public float shootCooldown = 2f; // Time between shots

    private float lastShootTime; // Time when the last shot was fired
    private NavMeshAgent agent; // NavMeshAgent for movement
    private Animator anim; // Animator for animations

    public Transform[] patrolPoints; // Points to patrol between
    private int PatrolIndex;

    AudioSource audioSource; // Audio source for sound effects
    public AudioClip shootSound; // Sound to play when shooting
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootRange)
        {
            anim.SetBool("walking", false);
            currentState = EnemyState.shoot;
            agent.isStopped = true; // Stop moving when shooting
            LookAtPlayer(); // Look at the player before shooting
            if(Time.time >= lastShootTime + shootCooldown)
            {
                Fire();
                lastShootTime = Time.time; // Update the last shoot time
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
            Patrol();
        }
    }

    void Patrol()
    {
        anim.SetBool("walking", true);
        agent.isStopped = false; // Resume movement when not shooting

        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            PatrolIndex = (PatrolIndex + 1) % patrolPoints.Length; // Cycle through patrol points
            agent.SetDestination(patrolPoints[PatrolIndex].position);
        }
        {
            
        }
    }
    void Fire()
    {
        if(bulletPrefab != null && BulletSpawnPoint != null)
        {
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound); // Play shooting sound
            }
            Instantiate(bulletPrefab, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
            anim.SetTrigger("Shoot");
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        
        Vector3 leftOffSet = Quaternion.Euler(0, -60, 0) * direction; // Adjust the direction to face the player
        leftOffSet.y = 0; // Keep the y component zero to avoid tilting up or down

        transform.rotation = Quaternion.LookRotation(leftOffSet);
    }
}
