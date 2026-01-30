using UnityEngine;

public abstract class Health : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected float invincibilityTime = 0.3f;

    protected int currentHealth;
    protected bool isInvincible;
    protected bool isDead;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead || isInvincible)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
            OnHit();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    protected virtual void OnHit() { }

    private System.Collections.IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}