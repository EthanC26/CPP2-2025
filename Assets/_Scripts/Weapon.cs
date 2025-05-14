using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
internal class Weapon : MonoBehaviour
{
    

    Rigidbody rb;
    BoxCollider bc;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    public void Equip(Collider playerCollider, Transform weaponAttachPoint)
    {
        //setting our rb to kinematic because we dont want to move via physics anymore
        rb.isKinematic = true;
        //setting our bc to be a trigger so we are not blocked by the sword collision
        bc.isTrigger = true;
        transform.SetParent(weaponAttachPoint);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Physics.IgnoreCollision(playerCollider, bc);
    }

    public void Drop(Collider playerCollider, Vector3 playerForward)
    {
        transform.parent = null;
        rb.isKinematic = false;
        bc.isTrigger = false;
        rb.AddForce(playerForward * 10, ForceMode.Impulse);
        StartCoroutine(DropCoolDown(playerCollider));
    }

    IEnumerator DropCoolDown(Collider playerCollider)
    {
        yield return new WaitForSeconds(2);

        //enable collsions
        Physics.IgnoreCollision(playerCollider, bc, false);
    }
    
}