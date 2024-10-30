using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeTemplate
{


    public class EnemyMage : Enemy
    {
        public ParticleSystem warpEffect;

        private new void FixedUpdate()
        {
            if (knockbacked || !canMove || stunned) return;

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

        public new void TakeDamage(DamageTaken damageTaken)
        {
            base.TakeDamage(damageTaken);

            if (!canMove || stunned) return;

            // Chance to teleport
            if (dashCD <= 0) Warp();
        }

        private void Warp()
        {
            // Effect
            warpEffect.transform.position = transform.position;
            warpEffect.Play();

            // Warp in radius
            dashPosition = transform.position + (Vector3)(Vector3.one * (Random.insideUnitCircle.normalized * dashRange));

            // Raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dashPosition - transform.position, dashRange, dashMask);

            // Warp
            if (hit)
            {
                if (hit.collider.gameObject.tag == "Border" || hit.collider.gameObject.layer == 13)
                {
                    Vector3 offset = ((Vector3)hit.point - transform.position).normalized;
                    transform.position = (Vector3)hit.point;
                }
                else transform.position = dashPosition;
            }
            else transform.position = dashPosition;

            flipPass = true;
            dashCD = baseDashCD;
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
