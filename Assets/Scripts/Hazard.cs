using System.Collections;
using UnityEngine;


namespace RoguelikeTemplate
{

    public class Hazard : MonoBehaviour
    {
        public bool dealsDamage = false;
        public bool knockback;
        public int damage = 2;
    
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!dealsDamage) return;

            if (other.TryGetComponent(out Entity entity))
            {
                int knockbackForce = 0;
                if (knockback) knockbackForce = 5;

                DamageTaken damageTaken;
                if (knockback) damageTaken = new DamageTaken(damage, knockbackForce, entity.transform.position - transform.position, 1);
                else damageTaken = new DamageTaken(damage, 0, Vector3.zero, 1);

                if (entity is Player)
                {
                    Player player = entity.GetComponent<Player>();

                    if (!player.dashing && player.dashInvincibility <= 0)
                    {
                        player.TakeDamage(damageTaken);
                        player.swordInvincible = true;
                    }
                    else
                    {
                        if (player.gameClass == Entity.GameClass.Knight && player.armed)
                        {
                            player.BreakCharge();
                            player.TakeDamage(damageTaken);
                            player.swordInvincible = true;
                        }
                    }

                    return;
                }
                
                else if (entity is EnemyKnight) entity.GetComponent<EnemyKnight>().TakeDamage(damageTaken);
                else if (entity is EnemyArcher) entity.GetComponent<EnemyArcher>().TakeDamage(damageTaken);
                else if (entity is EnemyMage) entity.GetComponent<EnemyMage>().TakeDamage(damageTaken);
                else if (entity is EnemySlime) entity.GetComponent<EnemySlime>().TakeDamage(damageTaken);
                else if (entity is TargetDummy) entity.GetComponent<TargetDummy>().TakeDamage(damageTaken);
                else if (entity is Prop) entity.GetComponent<Prop>().TakeDamage(damageTaken);
                //else entity.TakeDamage(damageTaken);

                entity.swordInvincible = true;
            }
        }

        public void EnableTrap() => dealsDamage = !dealsDamage;
    }
}
