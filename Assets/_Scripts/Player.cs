
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour, ProjectActions.IOverworldActions
{
    ProjectActions input;
    CharacterController cc;
    Camera mainCam;
    Animator anim;
    AudioSource audioSource;

    [Header("Collision Mask")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private Transform raycastOriginPoint;

    [Header("Movement Variables")]
    [SerializeField] private float initSpeed = 5.0f;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float moveAccel = 0.2f;
    [SerializeField] private float rotationSpeed = 30.0f;
    private float curSpeed = 5.0f;

    [Header("Jump Variables")]
    [SerializeField] private float jumpHeight = 0.1f;
    [SerializeField] private float jumpTime = 0.7f;

    public GameObject[] attackPrefab;

    //values clculated using jump height and jump time
    private float timeToJumpApex; //jumpTime / 2
    private float initJumpVelocity;

    //weapon variables
    [Header("Weapon Variables")]
    [SerializeField] private Transform weaponAttachPoint;
    [SerializeField] private Transform gunAttachPoint;
    Weapon weapon = null;
    Weapon SMG = null;
    Weapon banger = null;

    //attack vaiables
    public bool uAttack = false;
    public bool attack = false;
    public bool hasHit = false;
    public bool ShootAttack = false;

    private Shoot shoot;

    //character Movement
    Vector2 direction;
    Vector3 velocity;

    public bool inView;

    //player audio 
    public AudioClip HitClip;
    public AudioClip DeathClip;
    public AudioClip ShootClip;
    public AudioClip JumpClip;
    public AudioClip CollectClip;
    //calculated based on our jump values - this is the Y velocity that we will apply
    private float gravity;

    private bool isJumpPressed = false;

    private float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shoot = GetComponentInChildren<Shoot>();
        if (!shoot)
        {
            Debug.LogError("Shoot component not found!");
        }
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        mainCam = Camera.main;

        timeToJumpApex = jumpTime / 2;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        initJumpVelocity = -(gravity * timeToJumpApex);
    }

    void OnEnable()
    {
        input = new ProjectActions();
        input.Enable();
        input.Overworld.SetCallbacks(this);
    }
    void OnDisable()
    {
        input.Disable();
        input.Overworld.RemoveCallbacks(this);
    }
    #region Input Function
    public void OnJump(InputAction.CallbackContext context) => isJumpPressed = context.ReadValueAsButton();

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed) direction = context.ReadValue<Vector2>();
        if (context.canceled) direction = Vector2.zero;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (weapon)
        {
            weapon.Drop(GetComponent<Collider>(), transform.forward);
            weapon = null;
        }
        else if (SMG)
        {
            SMG.Drop(GetComponent<Collider>(), transform.forward);
            SMG = null;
        }
        else if (banger)
        {
            banger.Drop(GetComponent<Collider>(), transform.forward);
            banger = null;
        }
        else Debug.Log("No weapon to drop");
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log($"attacking");
        if (weapon)
        {
            attack = true;
            hasHit = false;
            elapsedTime = 0;
        }
        else if (!SMG && !weapon && !banger)
        {
            uAttack = true;
            elapsedTime = 0;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (SMG)
        {
            ShootAttack = true;
            elapsedTime = 0;
            audioSource.PlayOneShot(ShootClip);

        }
        else if (banger)
        {
            ShootAttack = true;
            elapsedTime = 0;
            audioSource.PlayOneShot(ShootClip);
        }

        else Debug.Log("No gun to shoot with");
    }

    #endregion

    void Update()
    {

        anim.SetBool("unarmedAttack", uAttack);
        anim.SetBool("Attack", attack);
        anim.SetBool("Shooting", ShootAttack);
        Vector2 groundVel = new Vector2(velocity.x, velocity.z);
        anim.SetFloat("vel", groundVel.magnitude);

        if (!raycastOriginPoint)
            return;

        Ray ray = new Ray(raycastOriginPoint.transform.position, transform.forward);
        RaycastHit hitInfo;


        Debug.DrawLine(raycastOriginPoint.transform.position, raycastOriginPoint.transform.position + (transform.forward * 10.0f),
            Color.red);

        if (Physics.Raycast(ray, out hitInfo, 10.0f, collisionMask))
        {
            //if true hitInfo will have something to output
            Debug.Log(hitInfo.transform.gameObject);
            //make so just makes enemy visable/freeze
            if (hitInfo.transform.CompareTag("enemy"))
            {
                inView = true;
            }
            else inView = false;

        }
        else inView = false;

        if (elapsedTime >= 0.5)
        {
            uAttack = false;
            attack = false;
            ShootAttack = false;
        }
        elapsedTime += Time.deltaTime;


    }

    void FixedUpdate()
    {
        Vector3 desiredMoveDirection = ProjectedMoveDirection();
        cc.Move(UpdateCharacterVelocity(desiredMoveDirection));

        //rotate towards direction of movement
        if (direction.magnitude > 0)
        {
            float timeStep = rotationSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), timeStep);
        }
    }
    private Vector3 ProjectedMoveDirection()
    {
        //grab our fwd and right vectors for camera relative movement 
        Vector3 camreaRight = mainCam.transform.right;
        Vector3 camreaForward = mainCam.transform.forward;

        //remove yaw rotation
        camreaForward.y = 0;
        camreaRight.y = 0;

        camreaForward.Normalize();
        camreaRight.Normalize();

        return camreaForward * direction.y + camreaRight * direction.x;
    }

    private Vector3 UpdateCharacterVelocity(Vector3 desiredDirection)
    {
        if (direction == Vector2.zero) curSpeed = initSpeed;

        velocity.x = desiredDirection.x * curSpeed;
        velocity.z = desiredDirection.z * curSpeed;

        curSpeed += moveAccel * Time.fixedDeltaTime;
        curSpeed = Mathf.Clamp(curSpeed, initSpeed, maxSpeed);

        if (!cc.isGrounded) velocity.y += gravity * Time.fixedDeltaTime;
        else velocity.y = CheckJump();

        return velocity;
    }

    private float CheckJump()
    {
        anim.SetBool("Jumping", isJumpPressed);

        if (isJumpPressed)
        {
            audioSource.PlayOneShot(JumpClip);
            return initJumpVelocity;

        }
        else return -cc.minMoveDistance;

    }

    public void ResetPlayerState(Vector3 newPosition, Quaternion newRotation)
    {
        cc.enabled = false;  // Temporarily disable to safely move CharacterController
        transform.position = newPosition;
        transform.rotation = newRotation;
        velocity = Vector3.zero;    // reset velocity so no unwanted motion happens
        direction = Vector2.zero;   // reset input direction
        cc.enabled = true;         // Re-enable
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("weapon") && weapon == null && SMG == null && banger == null)
        {
            audioSource.PlayOneShot(CollectClip);
            weapon = hit.gameObject.GetComponent<Weapon>();
            weapon.Equip(GetComponent<Collider>(), weaponAttachPoint);

        }

        if (hit.collider.CompareTag("SMG") && weapon == null && SMG == null && banger == null)
        {
            if (shoot != null)
            {
                audioSource.PlayOneShot(CollectClip);
                shoot.SetGun(Shoot.GunType.SMG);
            }
            SMG = hit.gameObject.GetComponent<Weapon>();
            SMG.Equip(GetComponent<Collider>(), gunAttachPoint);

        }

        if (hit.collider.CompareTag("Banger") && weapon == null && banger == null && SMG == null)
        {
            if (shoot != null)
            {
                audioSource.PlayOneShot(CollectClip);
                shoot.SetGun(Shoot.GunType.Banger);
            }
            banger = hit.gameObject.GetComponent<Weapon>();
            banger.Equip(GetComponent<Collider>(), gunAttachPoint);
        }

        if (hit.gameObject.CompareTag("endPoint"))
        {
            string sceneName = (SceneManager.GetActiveScene().name.Contains("Game")) ? "Victory" : "Game";
            SceneManager.LoadScene(sceneName);

            Debug.Log("YOU WIN!!!");
        }
        Debug.Log(hit.gameObject);

        if (hit.gameObject.CompareTag("Powerup"))
        {
            Destroy(hit.gameObject);
            Debug.Log("PowerUp Hit!!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            GameManager.Instance.SaveGame();
            Debug.Log("Checkpoint Set!");
        }
    }

    public void PlayHitSound()
    {
        

        if (audioSource != null && HitClip != null)
        {
            audioSource.PlayOneShot(HitClip);
        }
        else
        {
            Debug.LogWarning("AudioSource or HitClip is not set!");
        }
    }

    
    public void PlayDeathSound()
    {
        if (audioSource != null && DeathClip != null)
        {
            audioSource.PlayOneShot(DeathClip);
        }
        else
        {
            Debug.LogWarning("AudioSource or DeathClip is not set!");
        }
    }
}   
