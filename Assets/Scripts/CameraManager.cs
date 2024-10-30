using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RoguelikeTemplate
{

    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;

        public bool follow = true;
        public Transform target;
        public float smoothness = .125f;
        public Vector3 offset = new Vector3(0, 0, -10);

        public Vector2 minMaxX;
        public Vector2 minMaxY;

        void Start()
        {
            instance = this;
            SceneManager.activeSceneChanged += AdjustBounds; // Everytime a scene is changed - adjust bounds
        }

        private void FixedUpdate()
        {
            if (!follow || target == null) return;

            // Follow the target while also limiting movement within boundaries
            Vector3 desired = Vector3.Lerp(transform.position, target.position, smoothness) + offset;
            transform.position = new Vector3(Mathf.Clamp(desired.x, minMaxX.x, minMaxX.y), Mathf.Clamp(desired.y, minMaxY.x, minMaxY.y), -10);
        }

        public IEnumerator Shake(float duration, float force)
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            Vector3 originalPos = transform.position;
            Vector3 shakePos;

            while (duration > 0)
            {
                duration -= Time.deltaTime;
                float randomX = Random.Range(originalPos.x - 1 * force, originalPos.x + 1 * force);
                float randomY = Random.Range(originalPos.y - 1 * force, originalPos.y + 1 * force);
                shakePos = new Vector3(randomX, randomY, transform.position.z);
                transform.position = shakePos;
                yield return waitForFixedUpdate;
            }

            transform.position = originalPos;
        }

        private void AdjustBounds(Scene current, Scene next)
        {
            target = Player.instance.transform;

            // Find the dungeon Room of the level and set the camera boundaries to it's boundaries
            Room room = FindObjectOfType<Room>();
            if (room != null)
            {
                minMaxX = room.minMaxX;
                minMaxY = room.minMaxY;
            }
            else // If not in a dungeon room - don't limit the camera
            {
                minMaxX = new Vector2(-100, 100);
                minMaxY = new Vector2(-100, 100);
            }
        }
    }
}
