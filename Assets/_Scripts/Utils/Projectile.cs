
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        if (gameObject.CompareTag("eProj") && other.gameObject.CompareTag("enemy"))
        {
            NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
            BetterEnemy e = other.gameObject.GetComponent<BetterEnemy>();
            if (e != null)
            {
                if(agent == null)
                {
                   Debug.LogWarning("Enemy does not have a Rigidbody component, cannot apply force.");
                }
                e.DamageTaken();
                if(agent != null)
                {
                    Vector3 pushDirection = transform.forward;

                    float pushForce = 6.0f; // Adjust this value as needed

                    agent.isStopped = true;//stops the navmesh agent from moving

                    agent.Move(pushDirection * pushForce);

                    StartCoroutine(ResumeAgent(agent, 1.0f)); // Resume movement after a short delay

                    Debug.Log("Pushed enemy with projectile");
                }
                Destroy(gameObject);
                Debug.Log("shot enemy");
            }

        }
    }

    IEnumerator ResumeAgent(NavMeshAgent agent, float delay)
    {
        yield return new WaitForSeconds(delay);
        agent.isStopped = false; // Resume the NavMeshAgent after the delay
        Debug.Log("Resumed NavMeshAgent movement");
    }
}
