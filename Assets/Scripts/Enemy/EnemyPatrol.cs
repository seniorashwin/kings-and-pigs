using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.5f;
    public float patrolTime = 2f;
    public float idlePauseTime = 0.6f;

    [Header("Combat")]
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private BoxCollider2D playerDetector;
    [SerializeField] private Collider2D attackHitbox;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    float patrolTimer;
    float attackTimer;

    int direction = -1; // sprite faces LEFT by default
    bool isPausing;
    bool playerDetected;
    bool isAttacking;

    float detectorOffsetX;
    float hitboxOffsetX;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // store base detector offset (constant)
        detectorOffsetX = Mathf.Abs(playerDetector.offset.x);
        hitboxOffsetX = Mathf.Abs(((BoxCollider2D)attackHitbox).offset.x);
    }

    void OnEnable()
    {
        patrolTimer = patrolTime;
        attackTimer = 0f;
        isPausing = false;
        isAttacking = false;
        playerDetected = false;

        SetDirection(direction);
        anim.SetBool("isMoving", true);
        anim.SetBool("isAttacking", false);
    }

    void FixedUpdate()
    {
        // ---------- ATTACK LOOP ----------
        if (playerDetected && !isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;

            if (attackTimer <= 0f)
            {
                StartAttack();
                attackTimer = attackCooldown;
            }
        }

        // ---------- HARD STOP STATES ----------
        if (isAttacking || playerDetected || isPausing)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // ---------- PATROL ----------
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        anim.SetBool("isMoving", true);

        patrolTimer -= Time.fixedDeltaTime;
        if (patrolTimer <= 0f)
            StartIdlePause();
    }
    
    // PATROL TURN
    
    void StartIdlePause()
    {
        isPausing = true;
        anim.SetBool("isMoving", false);
        rb.linearVelocity = Vector2.zero;

        Invoke(nameof(EndIdlePause), idlePauseTime);
    }

    void EndIdlePause()
    {
        direction *= -1;
        SetDirection(direction);

        patrolTimer = patrolTime;
        isPausing = false;
        anim.SetBool("isMoving", true);
    }
    
    // DIRECTION + DETECTOR

    void SetDirection(int dir)
    {
        // visual flip
        sr.flipX = dir > 0;

        // detector flip
        Vector2 detectorOffset = playerDetector.offset;
        detectorOffset.x = detectorOffsetX * dir;
        playerDetector.offset = detectorOffset;

        // attack hitbox flip
        BoxCollider2D hitbox = (BoxCollider2D)attackHitbox;
        Vector2 hitboxOffset = hitbox.offset;
        hitboxOffset.x = hitboxOffsetX * dir;
        hitbox.offset = hitboxOffset;
    }

    
    // ATTACK
    void StartAttack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        anim.SetBool("isMoving", false);

        // hard restart attack animation (guarantees replay)
        anim.Play("pig_Attack", 0, 0f);
    }

    // Animation Event (LAST FRAME of pig_Attack)
    public void OnAttackEnd()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);

        if (!playerDetected)
            anim.SetBool("isMoving", true);
    }
    
    // DETECTOR CALLBACKS

    public void OnPlayerEnter()
    {
        playerDetected = true;
        attackTimer = 0f; // immediate attack
    }

    public void OnPlayerExit()
    {
        playerDetected = false;
    }
    
    public void EnableHitBox() => attackHitbox.enabled = true;
    public void DisableHitBox() => attackHitbox.enabled = false;
    
}
