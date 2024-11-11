using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow_Attack : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // ������ ������ ������Ʈ (ȭ��ǥ ��)
    [SerializeField] private Camera mainCamera; // ���� ī�޶�
    [SerializeField] private GameObject arrowPrefab; // �߻��� ȭ�� ������
    [SerializeField] private Transform arrowSpawnPoint; // ȭ���� �߻�� ��ġ
    [SerializeField] private float arrowSpeed = 20f; // ȭ�� �ӵ�

    private Vector2 gamepadInput; // ��ƽ �Է� �� �����
    private bool usingGamepad; // ���� ��ƽ�� ��� ������ Ȯ��

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // ���� ī�޶� �ڵ� �Ҵ�
        }
    }

    // ���콺 ������ ��ġ�� ���� ȸ��
    public void OnMouseAim(InputValue value)
    {
        if (!usingGamepad) // ��ƽ �Է� ���� �ƴ� ���� �۵�
        {
            Vector2 mousePosition = value.Get<Vector2>();

            // ��ũ�� ��ǥ -> ���� ��ǥ�� ��ȯ
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 direction = hitInfo.point - targetObject.position;
                direction.y = 0; // ���� ȸ���� ����

                if (direction != Vector3.zero)
                {
                    targetObject.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }

    // �����е��� ������ ��ƽ �Է��� ó��
    public void OnGamepadAim(InputValue value)
    {
        gamepadInput = value.Get<Vector2>();
        usingGamepad = gamepadInput != Vector2.zero; // ��ƽ�� ������ ���� true

        if (usingGamepad) // ��ƽ�� ������ ���� ȸ��
        {
            Vector3 direction = new Vector3(gamepadInput.x, 0, gamepadInput.y);
            if (direction != Vector3.zero)
            {
                targetObject.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    // ���� �Է� ó��
    public void OnAttack(InputValue value)
    {
        if (value.isPressed) // ���� �Է��� �����Ǹ�
        {
            FireArrow();
        }
    }

    // ȭ�� �߻� ����
    private void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = arrowSpawnPoint.forward * arrowSpeed; // ȭ���� ������ �ӵ��� �߻�
    }
}
