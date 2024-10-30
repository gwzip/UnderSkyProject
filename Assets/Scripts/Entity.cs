using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RoguelikeTemplate
{

    public class Entity : MonoBehaviour
    {
        public enum GameClass { Knight, Archer, Wizard, Monster }
        public GameClass gameClass;

        public Weapon weapon;

        [Header("Stats")]
        public int health;
        public int damage;
        public float speed;
        public float attackSpeed;
        public int criticalChance;
        public float knockbackMultiplier = 1;

        [Header("Stat Effects")]
        public bool stunned;
        public bool knockbacked;
        public bool canMove;

        [Header("Dash")]
        public float dashRange;
        public float dashCD;
        public float dashInvincibility;
        public bool dashing;
        public LayerMask dashMask;
        [HideInInspector] public Vector3 dashPosition;
        [HideInInspector] public float baseDashCD;

        [Header("Hands Manager")]
        public Transform back;
        public Transform freeHand;
        public Transform offHand;
        public Transform weaponCenter;
        protected Transform handL, handR;
        public List<SpriteRenderer> gfxComponents;
        public bool flipPass;
        public bool armed;

        [Header("Other")]
        public GameObject GFX;
        public Collider2D coreCollider;
        public BoxCollider2D triggerCollider;
        public GameObject stunEffect;
        public ParticleSystem xpEffect;
        public AudioClip hitEffect;
        public AudioClip critEffect;
        public Transform pivotPoint;

        public Rigidbody2D rb;
        public Animator animator;
        protected SpriteRenderer spriteRenderer;
        protected Shader shaderDefault;
        protected Shader shaderWhite;
        protected AudioSource audioSource;
        protected bool blocking;
        protected float baseShieldCD;
        protected int maxHealth;
        public bool flipped;
        public bool swordInvincible;

        // Health
        protected MySlider smartSlider;
        protected Text healthText;

        public void Start()
        {
            baseDashCD = dashCD;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            spriteRenderer = GFX.GetComponent<SpriteRenderer>();
            shaderWhite = Shader.Find("GUI/Text Shader");
            if (spriteRenderer != null) shaderDefault = spriteRenderer.material.shader;

            if (gfxComponents.Count > 0 )
            {
                handL = gfxComponents[2].transform;
                handR = gfxComponents[3].transform;
            }

            // Setup UI
            if (gameObject.layer == 11 || gameObject.layer == 15) return;

            if (GetComponentInChildren<Canvas>())
            {
                smartSlider = GetComponentInChildren<Canvas>().GetComponentInChildren<MySlider>();
                maxHealth = health;
                smartSlider.UpdateValues(health, maxHealth);
                if (healthText != null) healthText.text = health + "/" + maxHealth;
            }
        }

        public void FixedUpdate()
        {
            if (knockbacked || !canMove || stunned) return;
        }

        private void Update()
        {
            // Deal with cooldowns
            if (dashCD > 0) dashCD -= Time.deltaTime;
        }

        public void TakeDamage(DamageTaken damageTaken)
        {
            if (swordInvincible) return;

            // Knockback / 15 Because of rigidbody mass - every entity in the game has a mass of 15 (except the player who has 1) to prevent it from being pushed around
            if (damageTaken.knockback > 0) StartCoroutine(Knockback(damageTaken.knockback * 15, damageTaken.knockbackDirection));

            if (blocking) return;

            // Deal damage
            health -= damageTaken.damage;

            // Display damage
            if (damageTaken.damage < 500) // Show text if not executed
            {
                Text damageText = Instantiate(GameManager.instance.hitNumberPrefab, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-.35f, .5f), 0), Quaternion.identity).GetComponentInChildren<Text>();
                damageText.text = (damageTaken.damage).ToString();

                if (damageTaken.critMultiplier > 1)
                {
                    audioSource.clip = critEffect;
                    damageText.color = Color.yellow;
                }
                else audioSource.clip = hitEffect;
            }
        
            // Hit Effect
            foreach (SpriteRenderer sh in gfxComponents)
                sh.material.shader = shaderWhite;
            StartCoroutine(Shake(.075f, .3f));
            audioSource.Play();
            animator.SetFloat("Speed", 0);
            smartSlider.LoseHealth(health, .3f, .2f);
            if (healthText != null)
                healthText.text = health + "/" + maxHealth;

            if (health <= 0)
            {
                // Disable colliders
                Collider2D[] colliders = GetComponents<Collider2D>();
                for (int i = 0; i < colliders.Length; i++)
                    colliders[i].enabled = false;

                Invoke("XP", .15f);
                Destroy(gameObject, .3f);
            }
            else
            {
                // Hit Effect
                StartCoroutine(Shake(.075f, .3f));
                if (spriteRenderer != null) 
                    spriteRenderer.material.shader = shaderWhite;
                CancelInvoke("Default");
                Invoke("Default", .4f);
            }
        }

        public void Armed()
        {
            if (DialogueManager.instance != null && DialogueManager.instance.chatBox.activeInHierarchy || weapon == null) return;

            flipPass = true;
            armed = !armed;
            weapon.Holster();

            // Free hand
            if (armed)
            {
                if (flipped)
                {
                    handL.parent = weaponCenter;
                    handL.localPosition = weapon.handPlacement;
                    handL.rotation = Quaternion.identity;
                }
                else
                {
                    handR.parent = weaponCenter;
                    handR.localPosition = new Vector2(0.45f, 0);
                    handR.rotation = Quaternion.identity;
                }
            }
            else
            {
                if (flipped)
                {
                    handL.parent = offHand;
                    handL.localPosition = new Vector2(-.3f, -0.25f);
                    handL.rotation = Quaternion.identity;
                }
                else
                {
                    handR.parent = offHand;
                    handR.localPosition = new Vector2(.3f, -0.25f);
                    handR.rotation = Quaternion.identity;
                }
            }
        }

        public void FlipSprite(bool flip)
        {
            if (flip == flipped && !flipPass) return;

            flipPass = false;

            float multiplier = flip ? -1 : 1;

            if (armed)
            {
                if (flip) // Left
                {
                    handL.parent = weaponCenter;
                    handR.parent = offHand;
                    handL.localPosition = weapon.handPlacement;
                    handR.localPosition = new Vector2(.3f, -0.25f); // Free Hand position
                }
                else // Right
                {
                    handR.parent = weaponCenter;
                    handL.parent = offHand;
                    handR.localPosition = weapon.handPlacement;
                    handL.localPosition = new Vector2(-.3f, -0.25f); // Free Hand position
                }

                // Flip staff with wielder
                if (weapon.type == Weapon.Type.Staff)
                {
                    weapon.transform.localPosition = flip ? -Vector3.right * .7f : Vector3.right * .7f;
                    weapon.transform.localScale = Vector3.one;
                    weapon.GFX.flipX = flip;

                    if (flip)
                    {
                        handL.localPosition = new Vector2(-.5f, 0);
                        handL.transform.parent = weapon.GFX.transform;
                    }
                    else
                    {
                        handR.localPosition = new Vector2(.5f, 0);
                        handR.transform.parent = weapon.GFX.transform;
                    }

                    //if (flip) handL.localPosition = new Vector2(-weapon.handPlacement.x, weapon.handPlacement.y);
                    //else handR.localPosition = weapon.handPlacement;


                    //if (flip)
                    //{
                    //    weapon.transform.localPosition = -Vector3.right * .7f;
                    //    weapon.GFX.flipX = true;
                    //    handL.localPosition = new Vector2(-.5f, 0);
                    //    handL.transform.parent = weapon.GFX.transform;
                    //}
                    //else
                    //{
                    //    weapon.transform.localPosition = Vector3.right * .7f;
                    //    weapon.GFX.flipX = false;
                    //    handR.localPosition = new Vector2(.5f, 0);
                    //    handR.transform.parent = weapon.GFX.transform;
                    //}
                }
            }
            else
            {
                if (flip) // Left
                {
                    handL.parent = offHand;
                    handL.localPosition = new Vector2(-.3f, -0.25f);
                    handL.rotation = Quaternion.identity;
                }
                else // Right
                {
                    handR.parent = offHand;
                    handR.localPosition = new Vector2(.3f, -0.25f);
                    handR.rotation = Quaternion.identity;
                }
            }

            handL.rotation = Quaternion.identity;
            handR.rotation = Quaternion.identity;

            for (int i = 0; i < 6; i++) // 6 because gfxComponents has 6 elements + the weapon which shouldn't be flipped
                gfxComponents[i].flipX = flip;

            back.localScale = new Vector3(multiplier, 1, 1);

            flipped = flip;
        }

        public virtual void Stun(float time)
        {
            if (stunEffect != null) StartCoroutine(StunCo(time));
        }

        public IEnumerator StunCo(float time)
        {
            stunned = true;
            rb.isKinematic = true;
            stunEffect.SetActive(true);
            yield return new WaitForSecondsRealtime(time);
            stunEffect.SetActive(false);
            rb.isKinematic = false;
            stunned = false;
        }

        public IEnumerator Knockback(float force, Vector2 direction)
        {
            knockbacked = true;
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * force / 2 * knockbackMultiplier, ForceMode2D.Impulse);

            //yield return new WaitForSecondsRealtime(.1f);

            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + .1f)
            {
                yield return null;
            }

            rb.velocity = Vector2.zero;
            //yield return new WaitForSecondsRealtime(.2f);

            startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + .2f)
            {
                yield return null;
            }


            knockbacked = false;
        }

        public void BreakKnockback()
        {
            StopCoroutine("Knockback");
            rb.velocity = Vector2.zero;
            knockbacked = false;
        }

        public IEnumerator Shake(float force, float duration)
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            while (duration > 0)
            {
                duration -= Time.deltaTime;
                GFX.transform.localPosition = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)) * force;
                yield return waitForFixedUpdate;
            }

            GFX.transform.localPosition = Vector2.zero;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (knockbacked) BreakKnockback();
        }

        private void Default()
        {
            if (spriteRenderer != null) spriteRenderer.material.shader = shaderDefault;
            else
            {
                foreach (SpriteRenderer sh in gfxComponents)
                    sh.material.shader = shaderDefault;
            }
            swordInvincible = false;
        }

        protected void XP()
        {
            xpEffect.transform.parent = null;
            xpEffect.Play();
        }

        protected float AngleBetweenTwoPoints(Vector2 from, Vector2 to)
        {
            Vector2 direction = to - from;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}