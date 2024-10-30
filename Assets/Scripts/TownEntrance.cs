using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RoguelikeTemplate
{

    public class TownEntrance : MonoBehaviour
    {
        public Transform enterPoint, exitPoint;
        public bool entered;
        private Rigidbody2D playerRb;
        private BoxCollider2D coreCollider;
        private bool inRange;

        void Start()
        {
            playerRb = Player.instance.GetComponent<Rigidbody2D>();
            coreCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (inRange && !entered && Input.GetButtonDown("Interact"))
            {
                entered = true;
                StartCoroutine(Enter());
            }
        }

        public IEnumerator Enter()
        {
            entered = true;
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            operation.allowSceneActivation = false;

            if (Player.instance.armed) Player.instance.Armed();

            // Deal with player as he enters
            Player.instance.canMove = false;
            Player.instance.coreCollider.enabled = false;
            coreCollider.enabled = false;
            Player.instance.Descend();

            if (enterPoint.position.x > Player.instance.transform.position.x) Player.instance.FlipSprite(false);
            else Player.instance.FlipSprite(true);

            Vector2 enterDirection = (enterPoint.position - Player.instance.transform.position).normalized;

            while (Vector2.Distance(enterPoint.position, Player.instance.transform.position) > .7f)
            {
                // Player enter
                playerRb.velocity = enterDirection * 100 * Time.fixedDeltaTime;

                yield return waitForFixedUpdate;
            }

            playerRb.velocity = Vector2.zero;

            yield return new WaitForSeconds(1);

            while (FadePanel.instance.group.alpha < 1)
            {
                FadePanel.instance.group.alpha += .1f;
                yield return waitForFixedUpdate;
            }

            // Change scene
            operation.allowSceneActivation = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>()) inRange = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>()) inRange = false;
        }
    }
}
