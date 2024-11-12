using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TowerStart : MonoBehaviour
{
    [SerializeField] private InputAction changeSceneAction; // �ν����Ϳ��� ������ �� �ִ� InputAction
    [SerializeField] private string targetSceneName = "TutorialScene"; // �̵��� ���� �̸�

    private void OnEnable()
    {
        // �׼��� Ȱ��ȭ�Ͽ� �Է��� ���� �� �ֵ��� �մϴ�.
        changeSceneAction.Enable();
        changeSceneAction.performed += OnChangeScene; // �Է��� �߻����� �� ������ �޼��� ���
    }

    private void OnDisable()
    {
        // �׼��� ��Ȱ��ȭ�Ͽ� �޸� ������ �����մϴ�.
        changeSceneAction.performed -= OnChangeScene;
        changeSceneAction.Disable();
    }
    private void OnChangeScene(InputAction.CallbackContext context)
    {
        // �� ���� ����
        Debug.Log("���� �����մϴ�!");
        SceneManager.LoadScene(targetSceneName); // ������ ������ �� ��ȯ
    }
}
