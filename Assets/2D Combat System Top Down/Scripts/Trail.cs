using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem2D
{
    public class Trail : Hazard
    {
        public float liveTime;
        public Sprite[] variations;

        IEnumerator Start()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            // Pick a random variation
            GetComponent<SpriteRenderer>().sprite = variations[Random.Range(0, variations.Length)];

            // Scale up effect
            if (transform.localScale.x < 1)
            {
                while (transform.localScale.x < 1)
                {
                    transform.localScale += Vector3.one * .1f;
                    yield return waitForFixedUpdate;
                }
                transform.localScale = Vector3.one;
            }

            // Satanic circles specific - wait a second before activating trap
            yield return new WaitForSeconds(.3f);
            dealsDamage = true;

            // Destroy after livetime is over
            Invoke("DisappearEffect", liveTime);
            Destroy(gameObject, liveTime + 1);
        }

        private void DisappearEffect()
        {
            dealsDamage = false;
            StartCoroutine(DisappearEffectCO());
        }

        private IEnumerator DisappearEffectCO()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            while (transform.localScale.x > 0)
            {
                transform.localScale -= Vector3.one * .1f;
                yield return waitForFixedUpdate;
            }
            transform.localScale = Vector3.zero;
        }
    }
}