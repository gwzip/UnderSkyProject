using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed;
    public float smoothing = 0.125f; // 추가된 부드러움 변수
    private Vector3 targetPosition;

    private void LateUpdate() // Update에서 LateUpdate로 변경
    {
        if (target != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            // 부드러운 움직임을 위해 Lerp 대신 SmoothDamp 사용
            Vector3 smoothPosition = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            this.transform.position = smoothPosition;
        }
    }
}
