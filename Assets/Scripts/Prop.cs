using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeTemplate
{
    public class Prop : Entity
    {
        public int min, max;
        public GameObject[] possibleLoot;
        public GameObject[] breakEffect;

        public new void TakeDamage(DamageTaken damageTaken)
        {
            // Hit Effect
            StartCoroutine(Shake(.075f, .3f));
            if (spriteRenderer != null) spriteRenderer.material.shader = shaderWhite;
            Invoke("Default", .3f);
            Invoke("DestroyProp", .3f);
        }

        private void DestroyProp()
        {
            // Break Effect
            for (int i = 0; i < breakEffect.Length; i++)
                Instantiate(breakEffect[i], transform.position, Quaternion.identity);

            // Drop random Loot
            int random = Random.Range(min, max);

            for (int i = 0; i < random; i++)
                Instantiate(possibleLoot[Random.Range(0, possibleLoot.Length)], transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

}
