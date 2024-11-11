using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwap : MonoBehaviour
{
    public List<GameObject> weapons;
    private int currentWeaponIndex = 0;

    public InputAction changeWeaponAction; // New Input System action for swapping

    void OnEnable()
    {
        changeWeaponAction.Enable();
        changeWeaponAction.performed += ctx => SwapWeapon();
    }

    void OnDisable()
    {
        changeWeaponAction.performed -= ctx => SwapWeapon();
        changeWeaponAction.Disable();
    }

    void Start()
    {
        if (weapons.Count > 0)
        {
            ActivateWeapon(currentWeaponIndex);
        }
    }
    public void Change(InputValue value)
    {
        if (value.isPressed) // 공격 입력이 감지되면
        {
            SwapWeapon();
        }
    }
    void SwapWeapon()
    {
        Debug.Log("SwapWeapon called"); // Log to check if the method is called
        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0; // Loop back to the first weapon
        }

        ActivateWeapon(currentWeaponIndex);
        Debug.Log("Current weapon index: " + currentWeaponIndex); // Log the current weapon index
    }

    void ActivateWeapon(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == index);
        }
        Debug.Log("Activated weapon: " + weapons[index].name); // Log the name of the activated weapon
    }
}
