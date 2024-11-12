using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.SceneScripts
{
    public class TowerStart : MonoBehaviour
    {
        [SerializeField] private string targetSceneName; // �̵��� ���� �̸�
        [SerializeField] private InputAction interactAction; // �ν����Ϳ��� ������ Input Action

        private bool playerInZone = false;

        private void OnEnable()
        {
            interactAction.Enable(); // Input Action Ȱ��ȭ
        }

        private void OnDisable()
        {
            interactAction.Disable(); // Input Action ��Ȱ��ȭ
        }

        private void Update()
        {
            // �÷��̾ ���� �ȿ� �ְ� ���ͷ�Ʈ �׼��� �߻��ϸ� �� ��ȯ
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