using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1, 20)] private float lifeTime = 1.0f;
    [SerializeField, Range(1, 20)] private int damage = 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void SetVelocity(Vector3 velocity)
    {
        GetComponent<Rigidbody>().linearVelocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("pProj"))
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if (e != null) 
            {
                e.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        if(gameObject.CompareTag("eProj") && collision.gameObject.CompareTag("player"))
        {
         
            Player player = collision.gameObject.GetComponent<Player>();
            player.lives--;
            Debug.Log("hit");
            Destroy(gameObject);

        }
    }
}
