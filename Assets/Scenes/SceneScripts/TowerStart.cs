using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.SceneScripts
{
    public class TowerStart : MonoBehaviour
    {
        [SerializeField] private string targetSceneName; // 이동할 씬의 이름
        [SerializeField] private InputAction interactAction; // 인스펙터에서 설정할 Input Action

        private bool playerInZone = false;

        private void OnEnable()
        {
            interactAction.Enable(); // Input Action 활성화
        }

        private void OnDisable()
        {
            interactAction.Disable(); // Input Action 비활성화
        }

        private void Update()
        {
            // 플레이어가 구역 안에 있고 인터렉트 액션이 발생하면 씬 전환
            if (playerInZone && interactAction.triggered)
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInZone = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInZone = false;
            }
        }
    }
}