using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem2D
{
    public class EnemyMage : Enemy
    {
        private new void FixedUpdate()
        {
            if (!canMove || knockbacked) return;

            // Walk
            if (Vector2.Distance(transform.position, target) > .1f)
            {
                direction = (target - transform.position).normalized;
                rb.velocity = direction * speed * Time.fixedDeltaTime;

                if (target.x > transform.position.x) FlipSprite(false);
                else FlipSprite(true);
            }
            else rb.velocity = Vector2.zero;

            if (Player.instance != null && Vector2.Distance(Player.instance.transform.position, transform.position) < aggroRange) // Player in range
            {
                // Set player as destination
                if (!armed) Armed();
                giveUpTime = baseGiveUpTime;

                // Attack if in range
                if (Vector2.Distance(Player.instance.transform.position, transform.position) < attackRange) weapon.Attack();
            }

            Wander();

            animator.SetFloat("Speed", rb.velocity.magnitude);
        }

        private new void Wander()
        {
            if (waitTime > 0) waitTime -= Time.deltaTime;
            else
            {
                // Go to point
                if (Vector2.Distance(transform.position, wanderPoint) > .1) target = wanderPoint;
                else // Reach point
                {
                    if (waitTime > 0) waitTime -= Time.deltaTime;
                    else
                    {
                        wanderPoint = spawnPoint + (Vector3)Random.insideUnitCircle * wanderRange;
                        waitTime = Random.Range(1, 4);
                    }
                }
            }
        }
    }
}