using UnityEngine;

public class PigEnemy : MonoBehaviour
{
    public int hp = 1;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeHit()
    {
        hp--;
        anim.SetTrigger("hit");
        if (hp <= 0)
            Die();
    }

    void Die()
    {
        anim.SetTrigger("dead");
        Destroy(gameObject, 0.4f);
    }
    
}
