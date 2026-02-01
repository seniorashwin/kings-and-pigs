using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
   public int damage = 1;

   void OnTriggerEnter2D(Collider2D other)
   {
      if (!other.CompareTag("PigEnemy")) return;
      
      other.GetComponent<EnemyHealth>()?.TakeHit(damage);
   }
   
}
