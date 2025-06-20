using Unity.Cinemachine;
using UnityEngine;

public class RotatePond : MonoBehaviour
{
    AudioSource audioSource;
    public CinemachineImpulseSource impulseSource;
    [SerializeField] private float rotationspeed = 10f;

    public AudioClip HealingClip;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (rotationspeed <= 0)
        {
            Debug.LogWarning("Roation speed should be greater than 0. setting defult value should be 10");
            rotationspeed = 10f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationspeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            audioSource.PlayOneShot(HealingClip);
            Debug.Log("Player entered the pond area");
            //Add any additional logic for when the player enters the pond area
            GameManager.Instance.Lives++;
            impulseSource.GenerateImpulse(2.0f); 
        }
        
    }
}
