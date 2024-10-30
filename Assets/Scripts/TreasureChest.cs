using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeTemplate
{

    public class TreasureChest : MonoBehaviour
    {
        public GameObject[] lootTable;
        public bool open;
        public float spitForce;
        public Sprite openGFX;
        public Shader shaderWhite;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Shader shaderDefault;
        private AudioSource audioSource;
        private bool invincible = true;

        void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            shaderDefault = spriteRenderer.material.shader;
            Invoke("WearOff", .5f); // Remove invincibility after .5f seconds
        }

        public void DropLoot()
        {
            for (int i = 0; i < lootTable.Length; i++) // For every object in LootTable
            {
                GameObject loot = Instantiate(lootTable[i].gameObject, transform.position, Quaternion.identity); // Spawn it
                loot.GetComponent<Collectable>().spitDown = true; // Spit it down
            }
        }

        public void Open()
        {
            if (open || invincible) return; // If it's already opened or is invincible, return;

            Invoke("Drop", .15f); // Drop loot

            // Hit feedback
            spriteRenderer.material.shader = shaderWhite;
            StartCoroutine(Shake(.075f, .15f));
            audioSource.Play();
            open = true;
        }

        public IEnumerator Shake(float force, float duration) // Shake object 
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            while (duration > 0)
            {
                duration -= Time.deltaTime;
                spriteRenderer.transform.localPosition = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * force;
                yield return waitForFixedUpdate;
            }

            spriteRenderer.transform.localPosition = Vector2.zero;
        }

        void Drop() // Visually open
        {
            spriteRenderer.material.shader = shaderDefault;
            animator.SetTrigger("open");
            spriteRenderer.sprite = openGFX;
            DropLoot();
            Invoke("Despawn", 1);
        }

        public void DestroyObject() => Destroy(gameObject);

        private void Despawn() => animator.SetTrigger("despawn");

        private void WearOff() => invincible = false;
    }
}