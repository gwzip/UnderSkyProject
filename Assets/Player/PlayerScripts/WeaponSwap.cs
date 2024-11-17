using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwap : MonoBehaviour
{
    public List<GameObject> weapons; // ���� ����Ʈ
    private int currentWeaponIndex = 0;

    public InputAction changeWeaponAction; // ���� ���� �Է� �׼�

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
        // ���� ����Ʈ�� ���Ⱑ ���� ��� ù ��° ���⸦ Ȱ��ȭ
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
            return; // ���Ⱑ ���ų� �ϳ��� ������ ���� ���� �Ұ�
        }

        // ���� ���� ����
        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0; // ���� �ε����� ����
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
            weapons[i].SetActive(i == index); // ���� ���⸸ Ȱ��ȭ
        }

        Debug.Log($"Activated weapon: {weapons[index].name}");
    }
}
