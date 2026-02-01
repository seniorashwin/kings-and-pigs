using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int hp = 2;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeHit(int damage)
    {
        hp -= damage;
        anim.SetTrigger("hit");

        if (hp <= 0)
            Die();
    }

    void Die()
    {
        anim.SetBool("isDead", true);
        Destroy(gameObject, 0.4f);
    }
}