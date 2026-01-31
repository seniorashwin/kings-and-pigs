using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;
    public float jumpForce = 5f;
    [SerializeField] Collider2D attackHitbox;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    private Vector2 moveInput;
    private bool isAttacking;
    bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if(isAttacking) return;
        rb.linearVelocity = new  Vector2(speed * moveInput.x, rb.linearVelocity.y); 
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        if (moveInput.x != 0)
        {
            float dir = Mathf.Sign(moveInput.x);
            transform.localScale = new Vector3(dir, 1f, 1f);
        }

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed || !grounded) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);    
        grounded = false;
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || isAttacking) return;
        isAttacking = true;
        anim.SetTrigger("attack");
    }

    void UpdateAnimator()
    {
        anim.SetBool("isMoving", Mathf.Abs(moveInput.x) > 0.1f);
        anim.SetBool("isGrounded", grounded);
        anim.SetFloat("verticalVelocity", rb.linearVelocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PigEnemy")) ;
        //other.GetComponent<EnemyPatrol>()?.TakeHit();

    }
    
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            grounded = true;
            anim.SetTrigger("land");
        }
    }
    public void OnAttackEnd()
    {
        anim.Play(PlayerAnim.Idle);
        isAttacking = false;
        
    }

    public void OnHitEnd()
    {
        anim.Play(PlayerAnim.Idle);
    }

    public void OnLandEnd()
    {
        anim.Play(PlayerAnim.Idle);
    }
    
    public void EnableHitbox() => attackHitbox.enabled = true;
    public void DisableHitbox() => attackHitbox.enabled = false;
    
}
