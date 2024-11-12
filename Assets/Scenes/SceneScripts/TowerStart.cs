using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TowerStart : MonoBehaviour
{
    [SerializeField] private InputAction changeSceneAction; // 인스펙터에서 설정할 수 있는 InputAction
    [SerializeField] private string targetSceneName = "TutorialScene"; // 이동할 씬의 이름

    private void OnEnable()
    {
        // 액션을 활성화하여 입력을 받을 수 있도록 합니다.
        changeSceneAction.Enable();
        changeSceneAction.performed += OnChangeScene; // 입력이 발생했을 때 실행할 메서드 등록
    }

    private void OnDisable()
    {
        // 액션을 비활성화하여 메모리 누수를 방지합니다.
        changeSceneAction.performed -= OnChangeScene;
        changeSceneAction.Disable();
    }
    private void OnChangeScene(InputAction.CallbackContext context)
    {
        // 씬 변경 로직
        Debug.Log("씬을 변경합니다!");
        SceneManager.LoadScene(targetSceneName); // 설정한 씬으로 씬 전환
    }
}
