using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] private Camera playerCamera;

    [SerializeField] private Transform SMGspawnPoint;

    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private float bulletSPeed = 30.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (!playerCamera)
        {
            playerCamera = Camera.main;
        }
        if (!SMGspawnPoint)
            Debug.Log($"please put on {gameObject.name}");
    }

    public void Fire()
    {
        
        Vector3 shootDirection = transform.forward;

        Projectile curProjectile = Instantiate(projectilePrefab, SMGspawnPoint.position, Quaternion.LookRotation(shootDirection));

        curProjectile.SetVelocity(shootDirection * bulletSPeed);

    }
}
