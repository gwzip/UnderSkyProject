using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickupSystem : MonoBehaviour
{
    public GameObject weaponToUnlock; // 특정 구역에서 획득할 무기
    public Collider2D pickupZone; // 무기 획득 구역 (2D 환경용)
    public InputAction pickupWeaponAction; // 무기 획득 입력 액션 (Inspector에서 설정 가능)

    private bool isInPickupZone = false; // 플레이어가 특정 구역에 있는지 여부
    private WeaponSwap weaponSwap; // WeaponSwap 스크립트를 참조

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
        // WeaponSwap 스크립트 참조
        weaponSwap = GetComponent<WeaponSwap>();
        if (weaponSwap == null)
        {
            Debug.LogError("WeaponPickupSystem: WeaponSwap 스크립트를 찾을 수 없습니다!");
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
        // WeaponSwap의 weapons 리스트에 새 무기를 추가
        weaponSwap.weapons.Add(weaponToUnlock);
        Debug.Log("New weapon unlocked: " + weaponToUnlock.name);

        weaponToUnlock = null; // 무기를 한 번 획득하면 더 이상 획득 불가
    }
}
