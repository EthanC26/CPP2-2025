using UnityEngine;

public class randomPickup : MonoBehaviour
{
    public GameObject[] prefabList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(prefabList[Random.Range(0, prefabList.Length)], transform.position, transform.rotation);
    }
}
