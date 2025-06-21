using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyProjectile : MonoBehaviour
{
    public float lifeTime = 1.0f; // Lifetime of the projectile in seconds
    public float speed = 10.0f; // Speed of the projectile
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the projectile after its lifetime
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move the projectile forward
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
        {
            Player player = other.GetComponent<Player>();

            player.PlayHitSound();
            GameManager.Instance.Lives -= 1; // Reduce player health by 1

            if (GameManager.Instance.Lives <= 0)
            {
                player.PlayDeathSound();
            }

            Animator playeranim = player.GetComponentInChildren<Animator>();
            if (playeranim != null)
            {
                Debug.Log("Player Hit");
                playeranim.SetTrigger("Hit");
            }

            Destroy(gameObject); // Destroy the projectile
            
        }
    }
}
