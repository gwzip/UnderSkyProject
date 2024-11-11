using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow_Attack : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // 방향을 설정할 오브젝트 (화살표 등)
    [SerializeField] private Camera mainCamera; // 메인 카메라
    [SerializeField] private GameObject arrowPrefab; // 발사할 화살 프리팹
    [SerializeField] private Transform arrowSpawnPoint; // 화살이 발사될 위치
    [SerializeField] private float arrowSpeed = 20f; // 화살 속도

    private Vector2 gamepadInput; // 스틱 입력 값 저장용
    private bool usingGamepad; // 현재 스틱을 사용 중인지 확인

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 메인 카메라 자동 할당
        }
    }

    // 마우스 포인터 위치에 따라 회전
    public void OnMouseAim(InputValue value)
    {
        if (!usingGamepad) // 스틱 입력 중이 아닐 때만 작동
        {
            Vector2 mousePosition = value.Get<Vector2>();

            // 스크린 좌표 -> 월드 좌표로 변환
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 direction = hitInfo.point - targetObject.position;
                direction.y = 0; // 수평 회전만 적용

                if (direction != Vector3.zero)
                {
                    targetObject.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }

    // 게임패드의 오른쪽 스틱 입력을 처리
    public void OnGamepadAim(InputValue value)
    {
        gamepadInput = value.Get<Vector2>();
        usingGamepad = gamepadInput != Vector2.zero; // 스틱을 움직일 때만 true

        if (usingGamepad) // 스틱을 움직일 때만 회전
        {
            Vector3 direction = new Vector3(gamepadInput.x, 0, gamepadInput.y);
            if (direction != Vector3.zero)
            {
                targetObject.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    // 공격 입력 처리
    public void OnAttack(InputValue value)
    {
        if (value.isPressed) // 공격 입력이 감지되면
        {
            FireArrow();
        }
    }

    // 화살 발사 로직
    private void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = arrowSpawnPoint.forward * arrowSpeed; // 화살을 지정된 속도로 발사
    }
}
