using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeTemplate
{

    public class Projectile : MonoBehaviour
    {
        public Weapon parentWeapon;
        public float speed;
        public float homingTime;
        public bool pooled;
        public bool arrow;
        public bool homing;
        public Transform homingTarget;

        [HideInInspector] public Collider2D coreCollider;
        [HideInInspector] public Vector2 direction;

        private Rigidbody2D rb;
        private bool dealsDamage = true;
        public ParticleSystem trailEffect;
        public ParticleSystem splashEffect;
        private float targetAngle;
        private Vector3 desiredRotation;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            coreCollider = GetComponent<Collider2D>();
            trailEffect = GetComponentInChildren<ParticleSystem>();
            splashEffect = transform.GetChild(1).GetComponent<ParticleSystem>();

            arrow = name.Contains("Arrow") ? true : false;
        }

        void FixedUpdate()
        {
            if (dealsDamage && !pooled)
            {
                if (homingTime > 0 && homingTarget != null) 
                {
                    // Rotate towards target
                    targetAngle = GameManager.instance.AngleBetweenTwoPoints(transform.position, homingTarget.position);
                    desiredRotation = Vector3.forward * targetAngle;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(desiredRotation), .125f);
                    homingTime -= Time.fixedDeltaTime;
                }
                rb.velocity = transform.right * speed;
            }
        }

        public void Pool(bool shouldPool)
        {
            if (!shouldPool) // Removing from pool
            {
                coreCollider.enabled = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                trailEffect.Play();
                dealsDamage = true;
            }
            else // Returning to pool
            {
                if (parentWeapon != null && parentWeapon.wielder != null)
                {
                    //Physics2D.IgnoreCollision(parentWeapon.wielder.coreCollider, coreCollider, false);
                    //Physics2D.IgnoreCollision(parentWeapon.wielder.triggerCollider, coreCollider, false);
                }
            
                Splash();
                trailEffect.Stop();
                parentWeapon = null;

                // Return to respective pool based on type
                if (arrow) GameManager.instance.arrowPool.Add(this);
                else GameManager.instance.magicPool.Add(this);

                // Remove from  scene
                transform.position = Vector3.one * 5000;
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                dealsDamage = false;
            }

            pooled = shouldPool;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!dealsDamage || pooled) return;

            if (collision.TryGetComponent(out Entity entity))
            {
                if (parentWeapon != null && parentWeapon.wielder != null && parentWeapon.wielder != entity)
                {
                    // Calculate Critical chance
                    int critMultiplier = 1;
                    if (Random.Range(0, 101) <= parentWeapon.wielder.criticalChance + parentWeapon.critChance) critMultiplier = 2;

                    DamageTaken damageTaken = new DamageTaken((parentWeapon.damage + parentWeapon.wielder.damage) * critMultiplier, parentWeapon.knockback, entity.transform.position - parentWeapon.wielder.transform.position, critMultiplier);
                    if (entity is Player) entity.GetComponent<Player>().TakeDamage(damageTaken);
                    else if (entity is BossNecromancer && parentWeapon.wielder == Player.instance) entity.GetComponent<BossNecromancer>().TakeDamage(damageTaken);
                    else if (entity is EnemyKnight) entity.GetComponent<EnemyKnight>().TakeDamage(damageTaken);
                    else if (entity is EnemyArcher) entity.GetComponent<EnemyArcher>().TakeDamage(damageTaken);
                    else if (entity is EnemyMage) entity.GetComponent<EnemyMage>().TakeDamage(damageTaken);
                    else if (entity is EnemySlime) entity.GetComponent<EnemySlime>().TakeDamage(damageTaken);
                    else if (entity is TargetDummy) entity.GetComponent<TargetDummy>().TakeDamage(damageTaken);
                    else if (entity is Prop) entity.GetComponent<Prop>().TakeDamage(damageTaken);
                    //else entity.TakeDamage(damageTaken);

                    if (entity.TryGetComponent(out Enemy enemy)) enemy.aggroRange = 15;

                    Pool(true);
                }
            }
            else if (collision.TryGetComponent(out TreasureChest chest))
            {
                chest.Open();
                if (!pooled) Pool(true); 
            }

            if (collision.TryGetComponent(out Weapon weapon) && weapon.isAttacking && weapon != parentWeapon) // Deflection
            {
                // Arrow
                if (arrow)
                {
                    //Physics2D.IgnoreCollision(parentWeapon.wielder.coreCollider, coreCollider, false);
                    //Physics2D.IgnoreCollision(parentWeapon.wielder.triggerCollider, coreCollider, false);
                    homingTarget = parentWeapon.wielder.transform;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, GameManager.instance.AngleBetweenTwoPoints(transform.position, homingTarget.position)));
                    parentWeapon = weapon;
                }
                else// Magic 
                {
                    if (!pooled) Pool(true); 
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!pooled && collision.collider.GetComponent<TargetDummy>()) return;

            if (!pooled) Pool(true);
        }

        private void Splash()
        {
            if (splashEffect != null)
            {
                splashEffect.transform.SetParent(null);
                splashEffect.Play();
                Invoke("ReturnSplash", 1);
            }
        }

        private void ReturnSplash()
        {
            splashEffect.transform.parent = transform;
            splashEffect.transform.localPosition = Vector3.zero;
        }
    }

    public class DamageTaken
    {
        public int damage, critMultiplier;
        public float knockback;
        public Vector2 knockbackDirection;

        public DamageTaken(int _damage, float _knockback, Vector2 _knockbackDirection, int _critMultiplier)
        {
            damage = _damage;
            knockback = _knockback;
            knockbackDirection = _knockbackDirection;
            critMultiplier = _critMultiplier;
        }
    }


}
