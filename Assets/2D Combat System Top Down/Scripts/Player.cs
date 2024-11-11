using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace CombatSystem2D
{
    public class Player : Entity
    {
        public static Player instance;

        public GameObject shieldPrefab;
        public Weapon[] weaponPrefabs;

        public int coins;
        public GameObject shield;
        private Vector2 movement;
        private Camera cam;
        private bool invincible;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(this);

            cam = Camera.main;
        }

        private new void Start()
        {
            base.Start();

            shaderDefault = gfxComponents[0].material.shader;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) Armed();

            if (Input.GetKeyDown(KeyCode.Alpha1)) GetWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) GetWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) GetWeapon(2);

            // Attack if have weapon which isn't holstered
            if (weapon != null && !weapon.holstered) { if (Input.GetMouseButton(0) || weapon.Qattack) weapon.Attack(); }

            if (armed)
            {
                if (cam.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x) FlipSprite(false);
                else FlipSprite(true);
            }
            else
            {
                if (movement.x > 0) FlipSprite(false);
                else if (movement.x < 0) FlipSprite(true);
            }
        }

        new void FixedUpdate()
        {
            if (!canMove || knockbacked) return;

            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rb.velocity = movement * speed * Time.fixedDeltaTime;
            animator.SetFloat("Speed", movement.magnitude);

            if (weapon != null)
            {
                if (armed)
                {
                    // Weapon follow cursor
                    Vector2 positionOnScreen = weapon.pivotPoint.position;
                    Vector2 mouseOnScreen = cam.ScreenToWorldPoint(Input.mousePosition);
                    if (!weapon.isAttacking) weapon.targetAngle = GameManager.instance.AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
                }
            }

        }

        public new void FlipSprite(bool flip)
        {
            base.FlipSprite(flip);

            int direction = flip ? 1 : 0;
            animator.SetFloat("direction", direction);

            if (armed)
            {
                if (flip) // Left
                {
                    if (shield != null)
                    {
                        shield.transform.parent = handR;
                        shield.transform.localPosition = Vector3.zero;
                        shield.transform.localRotation = Quaternion.identity;
                    }
                }
                else // Right
                {
                    if (shield != null) 
                    {
                        shield.transform.parent = handL;
                        shield.transform.localPosition = Vector3.zero;
                        shield.transform.localRotation = Quaternion.identity;
                    }
                }

                if (shield != null) shield.GetComponent<SpriteRenderer>().flipX = flip;
            }
        }

        public new void TakeDamage(DamageTaken damageTaken)
        {
            if (swordInvincible || invincible) return;

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
            animator.SetFloat("Speed", 0);
            CancelInvoke("Default");
            Invoke("Default", .4f);
            StartCoroutine(Invincibility());
        }

        private IEnumerator Invincibility()
        {
            invincible = true;
            swordInvincible = true;

            yield return new WaitForSeconds(.3f);
            int ticks = 10;
            int alpha;
            float time = .2f;

            while (ticks > 0)
            {
                if (ticks % 2 == 0)
                {
                    alpha = 0;
                    if (time > .06f) time -= .03f;
                }
                else alpha = 1;

                foreach (SpriteRenderer sprite in gfxComponents) sprite.color = new Color(1, 1, 1, alpha);
                if (weapon != null)
                {
                    weapon.GFX.color = new Color(1, 1, 1, alpha);

                    if (weapon.type == Weapon.Type.Bow)
                        weapon.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
                }

                if (shield != null) shield.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);

                yield return new WaitForSeconds(time);
                ticks--;
            }


            invincible = false;
            swordInvincible = false;
        }

        private void Default()
        {
            foreach (SpriteRenderer sh in gfxComponents)
                sh.material.shader = shaderDefault;
        }

        public new void Armed()
        {
            base.Armed();

            if (shield != null)
            {
                if (armed)
                {
                    shield.GetComponent<SpriteRenderer>().sortingOrder = 8;
                    shield.transform.parent = freeHand;
                    shield.transform.localPosition = new Vector3(.15f, -0.25f, 0);
                }
                else
                {
                    shield.GetComponent<SpriteRenderer>().sortingOrder = -2;
                    shield.transform.parent = back;
                    shield.transform.localPosition = Vector3.right * -0.225f;
                }
            }
        }

        public void GetWeapon(int index)
        {
            if (this.weapon != null) return;

            // Give player his weapon
            Weapon weapon = Instantiate(weaponPrefabs[index], transform.position, Quaternion.identity, weaponCenter).GetComponent<Weapon>();
            if (weapon.type == Weapon.Type.Sword) weapon.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            weapon.transform.localPosition = weapon.holdOffset;
            weapon.pivotPoint = pivotPoint;
            weapon.wielder = this;
            weapon.GFX = weapon.GetComponentInChildren<SpriteRenderer>();
            this.weapon = weapon;
            armed = true;
            flipPass = true;

            // Add to player's GFX components so weapon gets effected by whatever the player is effected (invincibility, damage, etc.)
            while (gfxComponents.Count > 6) gfxComponents.RemoveAt(gfxComponents.Count - 1);
            gfxComponents.Add(weapon.GFX);
            if (weapon.type == Weapon.Type.Bow) gfxComponents.Add(weapon.GetComponentsInChildren<SpriteRenderer>()[1]);

            // Handle shield
            if (weapon.type == Weapon.Type.Sword)
            {
                shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, freeHand);
                int multiplier = -1;
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x) multiplier = 1;
                shield.transform.localPosition = new Vector3(.15f * multiplier, -.25f, 0);
            }

            if (armed) Armed();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Collectable collectable))
            {
                coins += collectable.value;
                AudioSource.PlayClipAtPoint(collectable.pickUp, transform.position);
                Destroy(collectable.gameObject);
            }
        }
    }
}