using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RoguelikeTemplate
{

    public class Enemy : Entity
    {
        public GameObject[] possibleLoot;

        [Header("Behaviour ranges")]
        public float aggroRange;
        public float attackRange;
        public float wanderRange;
        public float giveUpTime;
        public float waitTime;
        public Vector3 spawnPoint;
        public Vector3 target;

        protected Vector2 direction;
        protected float baseGiveUpTime;
        protected Vector3 wanderPoint;
        protected Slider healthSlider;
        protected bool spawned;
        public float distance;
        public bool necromancerSummoned;

        protected new void Start()
        {
            base.Start();

            if (gfxComponents.Count > 0)
            {
                shaderDefault = gfxComponents[0].material.shader;
                foreach (SpriteRenderer sh in gfxComponents) sh.color = new Color(0, 0, 0, 0);
            }
            else // If enemy is only one sprite
            {
                shaderDefault = GFX.GetComponent<SpriteRenderer>().material.shader;
                GFX.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }

            if (handR != null)
            {
                // Adjust right hand to weapon
                handR.parent = weaponCenter;
                handR.localPosition = weapon.handPlacement;
            }

            coreCollider.enabled = false;
            triggerCollider.enabled = false;
            canMove = false;
            target = transform.position;
            spawnPoint = transform.position;
            baseGiveUpTime = giveUpTime;
            wanderPoint = spawnPoint + (Vector3)Random.insideUnitCircle * wanderRange;
            waitTime = Random.Range(2, 6);
            if (armed) Armed();
            Spawn();

            if (necromancerSummoned) aggroRange = 50;
        }

        protected void Wander()
        {
            if (waitTime > 0) waitTime -= Time.deltaTime;
            else
            {
                // Disarm when Wandering
                if (armed) Armed();

                // Go to point
                if (Vector2.Distance(transform.position, wanderPoint) > .1) target = wanderPoint;
                else // Reach point
                {
                    if (waitTime > 0) waitTime -= Time.deltaTime;
                    else
                    {
                        wanderPoint = spawnPoint + (Vector3)Random.insideUnitCircle * wanderRange;
                        waitTime = Random.Range(2, 6);
                    }
                }
            }
        }

        public new void TakeDamage(DamageTaken damageTaken)
        {
            if (!spawned) return;

            // When you take damage pull out weapon
            if (!armed) Armed();

            // Run basic TakeDamage
            base.TakeDamage(damageTaken);

            // Add level and XP
            if (health <= 0)
            {
                LevelManager.instance.RemoveEntity(this);
                if (!necromancerSummoned)
                {
                    DropLoot(3, 6);
                    XPManager.instance.AddXP(4);
                }
            }
        }

        public void DropLoot(int min, int max)
        {
            int random = Random.Range(min, max);

            for (int i = 0; i < random; i++)
                Instantiate(possibleLoot[Random.Range(0, possibleLoot.Length)], transform.position, Quaternion.identity);
        }

        public void Spawn()
        {
            if (gfxComponents.Count > 0) StartCoroutine(SpawnCo());
            else StartCoroutine(SpawnUnifiedCo());
        }

        private IEnumerator SpawnCo()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            foreach (SpriteRenderer sh in gfxComponents)
            {
                sh.material.shader = shaderWhite;
                sh.color = Color.black;
            }

            float alpha = 0;

            // From invisible to black
            while (alpha < 1)
            {
                alpha += .1f;
                foreach (SpriteRenderer sh in gfxComponents)
                    sh.color = new Color(0, 0, 0, alpha);

                yield return waitForFixedUpdate;
            }

            // Form black to white
            alpha = 0;
            while (alpha < 1)
            {
                alpha += .2f;
                foreach (SpriteRenderer sh in gfxComponents)
                    sh.color = new Color(alpha, alpha, alpha, 1);

                yield return waitForFixedUpdate;
            }

            // From white to normal
            foreach (SpriteRenderer sh in gfxComponents)
                sh.material.shader = shaderDefault;

            alpha = 0;
            while (alpha < 1)
            {
                alpha += .2f;
                foreach (SpriteRenderer sh in gfxComponents)
                    sh.color = new Color(alpha, alpha, alpha, 1);

                yield return waitForFixedUpdate;
            }

            yield return new WaitForSeconds(.2f);

            coreCollider.enabled = true;
            triggerCollider.enabled = true;
            canMove = true;
            spawned = true;
        }

        private IEnumerator SpawnUnifiedCo()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            SpriteRenderer GFXR = GFX.GetComponent<SpriteRenderer>();

            GFXR.material.shader = shaderWhite;
            GFXR.color = Color.black;

            float alpha = 0;

            // From invisible to black
            while (alpha < 1)
            {
                alpha += .1f;
                GFXR.color = new Color(0, 0, 0, alpha);

                yield return waitForFixedUpdate;
            }

            // Form black to white
            alpha = 0;
            while (alpha < 1)
            {
                alpha += .2f;
                GFXR.color = new Color(alpha, alpha, alpha, 1);

                yield return waitForFixedUpdate;
            }

            // From white to normal
            GFXR.material.shader = shaderDefault;

            alpha = 0;
            while (alpha < 1)
            {
                alpha += .2f;
                GFXR.color = new Color(alpha, alpha, alpha, 1);

                yield return waitForFixedUpdate;
            }

            yield return new WaitForSeconds(.2f);

            coreCollider.enabled = true;
            triggerCollider.enabled = true;
            canMove = true;
            spawned = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // if monster
            if (gameClass == GameClass.Monster)
            {
                if (collision.collider.TryGetComponent(out Player player))
                {
                    if (player.dashing && player.gameClass == GameClass.Knight) return;

                    DamageTaken damageTaken = new DamageTaken(damage, 10, collision.transform.position - transform.position, 1);
                    collision.collider.GetComponent<Player>().TakeDamage(damageTaken);
                }
            }
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
