using UnityEngine;
using UnityEngine.InputSystem;

public class Bow_Attack : MonoBehaviour
{
    public InputAction bowAttackAction; // 인스펙터에서 매핑을 위한 InputAction
    public GameObject arrowPrefab; // 화살 프리팹
    public Transform firePoint; // 화살이 발사될 위치

    public float maxChargeTime = 2f; // 최대 충전 시간
    public float minDamage = 10f;    // 최소 대미지
    public float maxDamage = 30f;    // 최대 대미지
    public float minRange = 5f;      // 최소 사거리
    public float maxRange = 15f;     // 최대 사거리
    public float minSpeed = 10f;     // 최소 속도
    public float maxSpeed = 30f;     // 최대 속도

    private float chargeTime;
    private bool isCharging;

    private void Awake()
    {
        if (bowAttackAction == null)
        {
            bowAttackAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        }
    }

    private void OnEnable()
    {
        bowAttackAction.Enable();
        bowAttackAction.started += ctx => StartCharging();
        bowAttackAction.canceled += ctx => ReleaseBow();
    }

    private void OnDisable()
    {
        bowAttackAction.Disable();
        bowAttackAction.started -= ctx => StartCharging();
        bowAttackAction.canceled -= ctx => ReleaseBow();
    }

    private void Update()
    {
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > maxChargeTime)
            {
                chargeTime = maxChargeTime;
            }

            Debug.Log("Charging: " + chargeTime); // 차징 상태 확인
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f;
        Debug.Log("Started charging.");
    }

    private void ReleaseBow()
    {
        if (!isCharging) return;

        isCharging = false;

        // 대미지, 사거리, 속도를 충전 시간에 따라 계산
        float damage = Mathf.Lerp(minDamage, maxDamage, chargeTime / maxChargeTime);
        float range = Mathf.Lerp(minRange, maxRange, chargeTime / maxChargeTime);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, chargeTime / maxChargeTime);

        // 화살 생성 및 초기화
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Arrow arrowScript = arrow.GetComponent<Arrow>();

            if (arrowScript != null)
            {
                arrowScript.SetProperties(damage, range, speed); // 대미지, 사거리, 속도 설정
                Debug.Log("Arrow fired with damage: " + damage + ", range: " + range + ", and speed: " + speed);
            }
            else
            {
                Debug.LogWarning("Arrow prefab does not have an Arrow component attached.");
            }
        }
        else
        {
            Debug.LogWarning("Arrow prefab or fire point is not assigned.");
        }

        chargeTime = 0f;
    }
}
