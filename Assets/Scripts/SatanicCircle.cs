using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeTemplate
{

    public class SatanicCircle : Hazard
    {
        public float liveTime;
        public ParticleSystem activateEffect;

        IEnumerator Start()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

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
            yield return new WaitForSeconds(1f);
            dealsDamage = true;
            if (activateEffect != null) activateEffect.Play();
        }
    }
}
