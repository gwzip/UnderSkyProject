using System.Collections;
using UnityEngine;


namespace CombatSystem2D
{
    public class EnemySlime : Enemy
    {
        public GameObject trailPrefab;
        private float trailCD = .035f;
        public float dashRange;

        private void Update()
        {
            if (trailCD > 0) trailCD -= Time.deltaTime;
        }

        private new void FixedUpdate()
        {
            if (!canMove) return;

            if (rb.velocity.magnitude > 5 && trailCD < 0)
            {
                // Make trail
                Instantiate(trailPrefab, transform.position, Quaternion.identity);
                trailCD = .035f;
            }

            // If player dies go into idle
            if (Player.instance == null || Player.instance.health <= 0)
                animator.SetTrigger("playerDeath");
        }

        public void Lunge() => StartCoroutine(LungeCO()); // Animator event 

        public IEnumerator LungeCO()
        {
            // Come to a full stop
            rb.velocity = Vector2.zero;
            rb.drag = 0;

            // Lunge
            rb.AddForce((Player.instance.transform.position - transform.position).normalized * dashRange, ForceMode2D.Impulse);
            yield return new WaitForSecondsRealtime(.1f);

            // Slow down
            coreCollider.enabled = true;
            rb.drag = 12;
        }
    }
}