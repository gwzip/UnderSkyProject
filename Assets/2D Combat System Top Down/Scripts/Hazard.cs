using System.Collections;
using UnityEngine;


namespace CombatSystem2D
{
    public class Hazard : MonoBehaviour
    {
        public bool knockback;
        public float knockbackForce = 5;
        public bool dealsDamage = false;
        public int damage = 2;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!dealsDamage) return;

            if (other.TryGetComponent(out Entity entity))
            {
                DamageTaken damageTaken;
                if (knockback) damageTaken = new DamageTaken(damage, knockbackForce, entity.transform.position - transform.position, 1);
                else damageTaken = new DamageTaken(damage, 0, Vector3.zero, 1);

                if (entity is Player)
                {
                    Player player = entity.GetComponent<Player>();

                    player.TakeDamage(damageTaken);
                    player.swordInvincible = true;
                    return;
                }
                else if (entity is EnemyKnight) entity.GetComponent<EnemyKnight>().TakeDamage(damageTaken);
                else if (entity is EnemyArcher) entity.GetComponent<EnemyArcher>().TakeDamage(damageTaken);
                else if (entity is EnemyMage) entity.GetComponent<EnemyMage>().TakeDamage(damageTaken);
                else if (entity is EnemySlime) entity.GetComponent<EnemySlime>().TakeDamage(damageTaken);

                entity.swordInvincible = true;
            }
        }

        public void EnableTrap() => dealsDamage = !dealsDamage;
    }
}