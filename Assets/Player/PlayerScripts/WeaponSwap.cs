using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwap : MonoBehaviour
{
    public List<GameObject> weapons; // 무기 리스트
    private int currentWeaponIndex = 0;

    public InputAction changeWeaponAction; // 무기 변경 입력 액션

    void OnEnable()
    {
        changeWeaponAction.Enable();
        changeWeaponAction.performed += OnSwapWeapon;
    }

    void OnDisable()
    {
        changeWeaponAction.performed -= OnSwapWeapon;
        changeWeaponAction.Disable();
    }

    void Start()
    {
        // 무기 리스트에 무기가 있을 경우 첫 번째 무기를 활성화
        if (weapons.Count > 0)
        {
            ActivateWeapon(currentWeaponIndex);
        }
        else
        {
            Debug.Log("No weapons available at the start.");
        }
    }

    private void OnSwapWeapon(InputAction.CallbackContext ctx)
    {
        SwapWeapon();
    }

    void SwapWeapon()
    {
        if (weapons.Count <= 1)
        {
            Debug.Log("Cannot swap weapons. No additional weapons available.");
            return; // 무기가 없거나 하나만 있으면 무기 변경 불가
        }

        // 무기 변경 로직
        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0; // 무기 인덱스를 루프
        }

        ActivateWeapon(currentWeaponIndex);
        Debug.Log($"Swapped to weapon index: {currentWeaponIndex}");
    }

    void ActivateWeapon(int index)
    {
        if (weapons.Count == 0)
        {
            Debug.LogWarning("No weapons to activate.");
            return;
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == index); // 현재 무기만 활성화
        }

        Debug.Log($"Activated weapon: {weapons[index].name}");
    }
}
