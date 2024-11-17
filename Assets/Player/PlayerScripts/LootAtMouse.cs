using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtMouse : MonoBehaviour
{
    public InputAction lookAction;
    public InputAction gamepadLookAction;
    private bool hasInput = false;
    private bool gamepadInputActive = false; // �����е� �Է� Ȱ��ȭ �÷���
    private float initialAngle;

    private void Start()
    {
        // �ʱ� ������ ����
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

        // �����е� �Է� ó��
        Vector2 gamepadInput = gamepadLookAction.ReadValue<Vector2>();
        if (gamepadInput.sqrMagnitude > 0.01f)
        {
            inputVector = Camera.main.ViewportToScreenPoint(new Vector3(0.5f + gamepadInput.x * 0.5f, 0.5f + gamepadInput.y * 0.5f, 0));
            gamepadInputActive = true; // �����е� �Է� Ȱ��ȭ
            hasInput = true;
        }
        else
        {
            gamepadInputActive = false; // �����е� �Է��� ������ �÷��� ��Ȱ��ȭ
        }

        // ���콺 �Է� ó�� (�����е� �Է��� Ȱ��ȭ�Ǿ� ���� ���� ����)
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
            // �Է� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputVector.x, inputVector.y, Camera.main.nearClipPlane));
            worldPosition.z = 0; // 2D ȯ�濡���� z���� 0���� ����

            // ���� ���
            Vector3 direction = (worldPosition - transform.position).normalized;

            // ���� ���
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 90�� ����

            // ������Ʈ ȸ�� ����
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
