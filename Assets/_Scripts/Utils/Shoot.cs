using UnityEngine;

public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] private Vector3 initShotVelocity = Vector3.zero;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Projectile projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (initShotVelocity == Vector3.zero)
        {
            Debug.Log("Init shot velocity has been changed to default");
            initShotVelocity.x = 7.0f;
        }
    }

    public void Fire()
    {
        Projectile curProjectile;
        curProjectile = Instantiate(projectilePrefab,spawnPoint.position, spawnPoint.rotation);
    }
}
