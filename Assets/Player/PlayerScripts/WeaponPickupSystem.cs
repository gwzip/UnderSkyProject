using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickupSystem : MonoBehaviour
{
    public GameObject weaponToUnlock; // Ư�� �������� ȹ���� ����
    public Collider2D pickupZone; // ���� ȹ�� ���� (2D ȯ���)
    public InputAction pickupWeaponAction; // ���� ȹ�� �Է� �׼� (Inspector���� ���� ����)

    private bool isInPickupZone = false; // �÷��̾ Ư�� ������ �ִ��� ����
    private WeaponSwap weaponSwap; // WeaponSwap ��ũ��Ʈ�� ����

    void OnEnable()
    {
        pickupWeaponAction.Enable();
        pickupWeaponAction.performed += ctx => TryPickupWeapon();
    }

    void OnDisable()
    {
        pickupWeaponAction.Disable();
        pickupWeaponAction.performed -= ctx => TryPickupWeapon();
    }

    void Start()
    {
        // WeaponSwap ��ũ��Ʈ ����
        weaponSwap = GetComponent<WeaponSwap>();
        if (weaponSwap == null)
        {
            Debug.LogError("WeaponPickupSystem: WeaponSwap ��ũ��Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == pickupZone)
        {
            isInPickupZone = true;
            Debug.Log("Player entered the pickup zone");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == pickupZone)
        {
            isInPickupZone = false;
            Debug.Log("Player exited the pickup zone");
        }
    }

    void TryPickupWeapon()
    {
        if (isInPickupZone && weaponToUnlock != null && weaponSwap != null)
        {
            PickupWeapon();
        }
        else if (!isInPickupZone)
        {
            Debug.Log("Player is not in the pickup zone!");
        }
        else
        {
            Debug.Log("No weapon to unlock or WeaponSwap is missing.");
        }
    }

    void PickupWeapon()
    {
        // WeaponSwap�� weapons ����Ʈ�� �� ���⸦ �߰�
        weaponSwap.weapons.Add(weaponToUnlock);
        Debug.Log("New weapon unlocked: " + weaponToUnlock.name);

        weaponToUnlock = null; // ���⸦ �� �� ȹ���ϸ� �� �̻� ȹ�� �Ұ�
    }
}
