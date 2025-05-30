using UnityEngine;
using static Shoot;

public class Shoot : MonoBehaviour
{
    //SMG GUN
    [SerializeField] private Transform SMGSpawnPoint;
    [SerializeField] private Projectile SMGProjectilePrefab;
    [SerializeField] private float SMGBulletSPeed = 30.0f;

    //BANGER GUN
    [SerializeField] private Transform bangerSpawnPoint;
    [SerializeField] private Projectile bangerProjectilePrefab;
    [SerializeField] private float bangerBulletSpeed = 30.0f;

    //track the gun
    public enum GunType 
    {
        SMG,
        Banger
    }

    [SerializeField] private GunType currentGun = GunType.SMG;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!SMGSpawnPoint) Debug.Log($"please put on {gameObject.name}");
        if(!bangerSpawnPoint) Debug.Log($"please put on {gameObject.name}");

    }

   public void SetGun(GunType newGun)
    {
        currentGun = newGun;
        Debug.Log("Switched to " + newGun);
    }
    public void Fire()
    {
        Transform spawnPoint = null;
        Projectile projectilePrefab = null;
        float bulletSpeed = 0.0f;

        switch(currentGun)
        {
            case GunType.SMG:
                spawnPoint = SMGSpawnPoint;
                projectilePrefab = SMGProjectilePrefab;
                bulletSpeed = SMGBulletSPeed;
                break;
            case GunType.Banger:
                spawnPoint = bangerSpawnPoint;
                projectilePrefab = bangerProjectilePrefab;
                bulletSpeed = bangerBulletSpeed;
                break;
        }

        if (spawnPoint == null || projectilePrefab == null)
        {
            Debug.LogWarning("Gun not set up correctly!");
            return;
        }


        Vector3 shootDirection = transform.forward;

        Projectile curProjectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.LookRotation(shootDirection));

        curProjectile.SetVelocity(shootDirection * SMGBulletSPeed);

        Debug.Log($"Fired {currentGun} from {spawnPoint.name} with speed {bulletSpeed}");

    }
}
