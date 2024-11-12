using UnityEngine;
using UnityEngine.InputSystem;

public class Bow_Attack : MonoBehaviour
{
    public InputAction bowAttackAction; // �ν����Ϳ��� ������ ���� InputAction
    public GameObject arrowPrefab; // ȭ�� ������
    public Transform firePoint; // ȭ���� �߻�� ��ġ

    public float maxChargeTime = 2f; // �ִ� ���� �ð�
    public float minDamage = 10f;    // �ּ� �����
    public float maxDamage = 30f;    // �ִ� �����
    public float minRange = 5f;      // �ּ� ��Ÿ�
    public float maxRange = 15f;     // �ִ� ��Ÿ�
    public float minSpeed = 10f;     // �ּ� �ӵ�
    public float maxSpeed = 30f;     // �ִ� �ӵ�

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

            Debug.Log("Charging: " + chargeTime); // ��¡ ���� Ȯ��
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

        // �����, ��Ÿ�, �ӵ��� ���� �ð��� ���� ���
        float damage = Mathf.Lerp(minDamage, maxDamage, chargeTime / maxChargeTime);
        float range = Mathf.Lerp(minRange, maxRange, chargeTime / maxChargeTime);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, chargeTime / maxChargeTime);

        // ȭ�� ���� �� �ʱ�ȭ
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Arrow arrowScript = arrow.GetComponent<Arrow>();

            if (arrowScript != null)
            {
                arrowScript.SetProperties(damage, range, speed); // �����, ��Ÿ�, �ӵ� ����
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
