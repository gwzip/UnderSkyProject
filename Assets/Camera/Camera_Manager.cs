using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed;
    public float smoothing = 0.125f; // �߰��� �ε巯�� ����
    private Vector3 targetPosition;

    private void LateUpdate() // Update���� LateUpdate�� ����
    {
        if (target != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            // �ε巯�� �������� ���� Lerp ��� SmoothDamp ���
            Vector3 smoothPosition = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            this.transform.position = smoothPosition;
        }
    }
}
