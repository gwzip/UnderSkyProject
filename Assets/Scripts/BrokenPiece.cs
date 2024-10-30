using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeTemplate
{

    public class BrokenPiece : MonoBehaviour
    {
        public Vector2 spitMinMax;

        private Rigidbody2D rb;
        private float aliveTime = 5;
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            Spit();
        }

        void Spit()
        {
            Vector2 spitDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            float spitForce = Random.Range(spitMinMax.x, spitMinMax.y);
            rb.AddForce(spitDirection * spitForce, ForceMode2D.Impulse);
        }

        private void FixedUpdate()
        {
            if (aliveTime > 0)
            {
                aliveTime -= Time.fixedDeltaTime;
                spriteRenderer.color = new Color(1, 1, 1, aliveTime);
            }
            else Destroy(gameObject);
        }
    }
}
