using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.5f;
    public float patrolTime = 2f;
    public float idlePauseTime = 0.6f;
    [SerializeField] BoxCollider2D playerDetector;
    
    
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    float timer;
    int direction = -1; // -1 = left (sprite default)
    bool isPausing;
    bool playerDetected;
    bool isAttacking;
    float detectorOffsetX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        detectorOffsetX = playerDetector.offset.x;

    }

    void OnEnable()
    {
        timer = patrolTime;
        isPausing = false;
        SetDirection(direction);
        anim.SetBool("isMoving", true);
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (playerDetected)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            anim.SetBool("isMoving", false);
            return;
        }
        
        if (isPausing)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        anim.SetBool("isMoving", true);

        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
            StartIdlePause();
    }

    void StartIdlePause()
    {
        isPausing = true;
        anim.SetBool("isMoving", false);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        Invoke(nameof(EndIdlePause), idlePauseTime);
    }

    void EndIdlePause()
    {
        direction *= -1;
        SetDirection(direction);

        timer = patrolTime;
        isPausing = false;
        anim.SetBool("isMoving", true);
    }

    void SetDirection(int dir)
    {
        // Sprite faces LEFT by default
        // Moving right → flipX true
        sr.flipX = dir > 0;
        
        //move detector to face movement direction
        Vector2 offset = playerDetector.offset;
        offset.x = Mathf.Abs(detectorOffsetX) * dir;
        playerDetector.offset = offset;
    }

    void StartAttack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        anim.SetBool("isMoving", false);
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
        
        if(!playerDetected)
            anim.SetBool("isMoving", true);
    }
    
    public void OnPlayerEnter()
    {
        if (isAttacking) return;
        
        playerDetected = true;
        StartAttack();

    }

    public void OnPlayerExit()
    {
        playerDetected = false;
    }
    
}