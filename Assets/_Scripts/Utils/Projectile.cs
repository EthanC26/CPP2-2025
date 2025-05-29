
using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1, 20)] private float lifeTime = 1.0f;
  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetVelocity(Vector3 velocity)
    {
        GetComponent<Rigidbody>().linearVelocity = velocity;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("pProj") && other.gameObject.CompareTag("enemy"))
        {
            BetterEnemy e = other.gameObject.GetComponent<BetterEnemy>();
            if (e != null)
            {
                e.DamageTaken();
                Destroy(gameObject);
                Debug.Log("shot enemy");
            }
        }
        //if (gameObject.CompareTag("eProj") && other.gameObject.CompareTag("player"))
        //{

        //    Player player = other.gameObject.GetComponent<Player>();
        //    //player.lives--;
        //    Debug.Log("hit");
        //    Destroy(gameObject);

        //}
    }
}
