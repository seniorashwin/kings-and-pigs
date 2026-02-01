using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;
    public float jumpForce = 5f;

    [Header("Combat")]
    [SerializeField] Collider2D attackHitbox;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    Vector2 moveInput;
    bool grounded;
    bool isAttacking;
    bool isHurt;
    bool isDead;
    public bool IsDead => isDead;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(isDead) return;
        anim.SetBool("isMoving", Mathf.Abs(moveInput.x) > 0.1f);
        anim.SetBool("isGrounded", grounded);
        anim.SetFloat("verticalVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if(isDead) return;
        if (isAttacking || isHurt) return;
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }
    
    public void SetHurtState(bool hurt) => isHurt = hurt;
    
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if(isDead) return;
        if(isAttacking || isHurt) return;
        
        moveInput = ctx.ReadValue<Vector2>();

        if (moveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1f, 1f);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(isDead) return;
        if (!ctx.performed || !grounded) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        grounded = false;
        anim.SetTrigger("jump");
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if(isDead) return;
        if (!ctx.performed || isAttacking || isHurt) return;

        isAttacking = true;
        anim.SetTrigger("attack");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Ground")) return;

        grounded = true;
        anim.SetTrigger("land");
    }

    // ===== Animation Events =====

    public void EnableHitbox() => attackHitbox.enabled = true;
    public void DisableHitbox() => attackHitbox.enabled = false;

    public void OnAttackEnd()
    {
        if (isDead) return;

        isAttacking = false;
        anim.Play(PlayerAnim.Idle);
    }

    public void OnLandEnd()
    {
        if (isDead) return;

        anim.Play(PlayerAnim.Idle);
    }

    public void OnDeath()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        
    }

}
