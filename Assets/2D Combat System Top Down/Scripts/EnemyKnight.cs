using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem2D
{
    public class EnemyKnight : Enemy
    {
        [Header("Circular movement")]
        public Transform circleTransform;
        private float circleAngle;
        private Vector2 circleDirection;
        private int radiusMultiplier = 1;

        // Safe zone
        private int safeMultiplier = 1;
        private Vector2 safeZoneDirection;

        private float timingOffset;

        private new void FixedUpdate()
        {
            if (!canMove || knockbacked) return;

            distance = Vector2.Distance(transform.position, target);

            // Movement
            if (distance > .1f)
            {
                direction = (target - transform.position).normalized;
                rb.velocity = direction * speed * Time.fixedDeltaTime;

                // Point weapon towards target
                if (weapon != null && Player.instance != null)
                {
                    // Weapon follow player
                    if (!weapon.isAttacking) weapon.targetAngle = AngleBetweenTwoPoints(weapon.pivotPoint.position, Player.instance.transform.position);
                }

                if (target.x > transform.position.x) FlipSprite(false);
                else FlipSprite(true);
            }
            else // If target is reached, stop
                rb.velocity = Vector2.zero;

            if (Player.instance != null && Vector2.Distance(Player.instance.transform.position, transform.position) < aggroRange) // Player in range
            {
                // Set player as destination
                if (!armed) Armed();
                target = Player.instance.transform.position;
                giveUpTime = baseGiveUpTime;

                // Circular movement
                circleAngle = AngleBetweenTwoPoints(transform.position, target);
                circleTransform.rotation = Quaternion.Euler(0, 0, circleAngle);
                circleDirection = circleTransform.up;

                // If you get too far away or out of the attack range - go closer
                if (distance > attackRange) safeMultiplier = 1;
                // Else if you get too close or close to the target - go away
                else if (distance < 1) safeMultiplier = -1;

                safeZoneDirection = (target - transform.position).normalized;

                // Overall movement
                rb.velocity = (circleDirection * radiusMultiplier + safeZoneDirection * safeMultiplier) * speed * Time.fixedDeltaTime;

                // Attack if in range
                if (Vector2.Distance(Player.instance.transform.position, transform.position) < attackRange) weapon.Attack();
            }
            else
            {
                if (giveUpTime > 0) // Wait
                {
                    giveUpTime -= Time.deltaTime;
                    target = transform.position;
                }
                else Wander();
            }

            animator.SetFloat("Speed", rb.velocity.magnitude);
        }

        public new void TakeDamage(DamageTaken damageTaken)
        {
            timingOffset = UnityEngine.Random.Range(0, .75f);

            base.TakeDamage(damageTaken);
        }
    }
}