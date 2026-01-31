using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private EnemyPatrol enemy;
    
    void Awake()
    {
        enemy = GetComponentInParent<EnemyPatrol>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            enemy.OnPlayerEnter();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            enemy.OnPlayerExit();
    }
}
