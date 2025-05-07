using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int health;
    [SerializeField] protected int maxHealth;
    protected Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        if (maxHealth <= 0) maxHealth = 5;

        health = maxHealth;
    }

   public virtual void TakeDamage(int DamageValue, DamageType damage = DamageType.Default)
    {
        health -= DamageValue;

        if(health <= 0)
        {
            anim.SetTrigger("Death");
            if (transform.parent != null) Destroy(transform.parent.gameObject);
            else Destroy(gameObject);
        }
    }

    public enum DamageType
    {
        Default,
        jumpedOn
    }    
}
