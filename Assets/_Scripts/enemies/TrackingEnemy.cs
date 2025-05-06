
using UnityEngine;

public class TrackingEnemy : Enemy
{
    [SerializeField] float eSpeed = 4;

    [SerializeField] private float timer = 5;
    private float bulletTime;
    public float bulletspeed = 20;
    public GameObject enemyBullet;
    public Transform spawnPoint;

    private Renderer rend;
    private Rigidbody rb;
   
    private Player player;

    private bool visable;

    private Transform playerLoction;

    //loads before start
    private void Awake() //less costly to load here instad of start
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        playerLoction = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        if (playerLoction != null)
        {
            player = playerLoction.GetComponent<Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerLoction);

        Sneek();
    }

    void Sneek()
    {
        if (player != null)
        {
            if (player.inView)
            {
                visable = true;
            }
            else visable = false;

            if (!visable)
            {
                rend.enabled = true;
                eSpeed = 4.0f;
                transform.position = Vector3.MoveTowards(transform.position, playerLoction.position, eSpeed * Time.deltaTime);
                ShootAtPlayer();

            }
            else if (visable)
            {
                rend.enabled = false;
                eSpeed = 0;
                rb.constraints = RigidbodyConstraints.FreezeAll;

            }
        }
    }
    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0) return;

        bulletTime = timer;

        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * bulletspeed);
        
    }
}
