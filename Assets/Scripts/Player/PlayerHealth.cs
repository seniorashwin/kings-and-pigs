using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnTime = 0.8f;
    
    
    int currentHealth;
    bool isInvulnerable;

    Animator anim;
    Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        isInvulnerable = false;
    }

    // ----------------------------------------------------

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;

        anim.SetTrigger("hit");

        // OPTIONAL knockback (safe even if force = 0)
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDirection * 6f, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(Invulnerability());
    }

    // ----------------------------------------------------

    System.Collections.IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(invulnTime);

        isInvulnerable = false;
    }

    void Die()
    {
        //prevent multiple death calls
        if(isInvulnerable) return;
        isInvulnerable = true;
        //aimator
        anim.SetBool("isDead", true);
        
        //disable movement 
        rb.linearVelocity = Vector2.zero;
        
        //disable player control 
        GetComponent<PlayerController>().OnDeath();
        
    }

    void OnDeathComplete()
    {
        //later
    }

    // ----------------------------------------------------
    // DEBUG / FUTURE UI
    // ----------------------------------------------------

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}