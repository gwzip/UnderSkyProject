using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtMouse : MonoBehaviour
{
    public InputAction lookAction;
    public InputAction gamepadLookAction;
    private bool hasInput = false;
    private bool gamepadInputActive = false; // 게임패드 입력 활성화 플래그
    private float initialAngle;

    private void Start()
    {
        // 초기 각도를 설정
        Vector3 initialDirection = transform.right;
        initialAngle = Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, initialAngle);
    }

    private void OnEnable()
    {
        lookAction.Enable();
        gamepadLookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
        gamepadLookAction.Disable();
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        // 게임패드 입력 처리
        Vector2 gamepadInput = gamepadLookAction.ReadValue<Vector2>();
        if (gamepadInput.sqrMagnitude > 0.01f)
        {
            inputVector = Camera.main.ViewportToScreenPoint(new Vector3(0.5f + gamepadInput.x * 0.5f, 0.5f + gamepadInput.y * 0.5f, 0));
            gamepadInputActive = true; // 게임패드 입력 활성화
            hasInput = true;
        }
        else
        {
            gamepadInputActive = false; // 게임패드 입력이 없으면 플래그 비활성화
        }

        // 마우스 입력 처리 (게임패드 입력이 활성화되어 있지 않을 때만)
        if (!gamepadInputActive && lookAction != null)
        {
            inputVector = lookAction.ReadValue<Vector2>();
            if (inputVector.sqrMagnitude > 0.01f)
            {
                hasInput = true;
            }
        }

        if (hasInput)
        {
            // 입력 좌표를 월드 좌표로 변환
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputVector.x, inputVector.y, Camera.main.nearClipPlane));
            worldPosition.z = 0; // 2D 환경에서는 z축을 0으로 설정

            // 방향 계산
            Vector3 direction = (worldPosition - transform.position).normalized;

            // 각도 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 90도 보정

            // 오브젝트 회전 적용
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
