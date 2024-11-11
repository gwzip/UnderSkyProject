using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CombatSystem2D
{
    public class MySlider : MonoBehaviour
    {
        public float shakeForce = 6;
        public bool hide;

        public ParticleSystem fillEffect;
        public RectTransform fill;
        public Vector3 effectPosition;

        private Slider slider;
        public Slider damageSlider;
        private CanvasGroup canvasGroup;
        public Camera cam;
        private Vector3 originalPosition;
        private bool shake;
        private float hideTime = 0;
        private float afkTime = 5;
        private Coroutine runningCoroutine;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            slider = GetComponent<Slider>();
            originalPosition = transform.localPosition;
        }

        void FixedUpdate()
        {
            if (shake) transform.localPosition = new Vector2(originalPosition.x + Random.Range(-1, 2) * shakeForce, originalPosition.y + Random.Range(-1, 2) * shakeForce);
        }

        private void Update()
        {
            if (!hide) return;

            if (afkTime > 0) afkTime -= Time.deltaTime;
            else
            {
                if (hideTime > 0)
                {
                    hideTime -= 3 * Time.deltaTime;
                    canvasGroup.alpha = hideTime;
                }
            }
        }

        public void UpdateValues(int current, int max)
        {
            slider = GetComponent<Slider>();

            slider.maxValue = max;
            slider.value = current;
            if (damageSlider != null)
            {
                damageSlider.maxValue = max;
                damageSlider.value = current;
            }
        }

        public void LoseHealth(int desire, float damageTime, float increments)
        {
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);

            runningCoroutine = StartCoroutine(LoseHealthCoroutine(desire, damageTime, increments));
        }

        private IEnumerator LoseHealthCoroutine(int desire, float damageTime, float increments)
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            if (hide)
            {
                hideTime = 1;
                afkTime = 5;
                canvasGroup.alpha = 1;
            }

            // Instant slider change
            slider.value = desire;

            // Wait time 
            yield return new WaitForSeconds(damageTime);

            // Smoothly decrease white stuff
            while (damageSlider.value > desire)
            {
                damageSlider.value -= increments;

                yield return waitForFixedUpdate;
            }

            damageSlider.value = desire;

            runningCoroutine = null;
        }
    }
}