using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weapons; // ���� ������Ʈ ����Ʈ
    private int currentWeaponIndex = 0;
    private IWeapon currentWeapon;

    // InputAction ���� ����
    public InputActionAsset inputActions;
    private InputAction changeWeaponAction;
    private InputAction attackAction;

    private void OnEnable()
    {
        // InputAction Ȱ��ȭ
        changeWeaponAction.Enable();
        attackAction.Enable();
    }

    private void OnDisable()
    {
        // InputAction ��Ȱ��ȭ
        changeWeaponAction.Disable();
        attackAction.Disable();
    }

    private void Start()
    {
        // InputActionAsset���� �׼� ��������
        var actionMap = inputActions.FindActionMap("Player"); // 'Player'��� Action Map�� ã�Ƽ� ���
        changeWeaponAction = actionMap.FindAction("Change");
        attackAction = actionMap.FindAction("Attack");

        // �ʱ� ���� ����
        EquipWeapon(currentWeaponIndex);

        // InputAction�� �̺�Ʈ ���
        changeWeaponAction.performed += _ => ChangeWeapon();
        attackAction.performed += _ => UseWeapon();
    }

    private void ChangeWeapon()
    {
        // ���� ���� ��Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(false);

        // ���� ����� �ε��� ����
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;

        // ���ο� ���� ����
        EquipWeapon(currentWeaponIndex);
    }

    private void UseWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Use();
        }
    }

    private void EquipWeapon(int index)
    {
        weapons[index].SetActive(true);
        currentWeapon = weapons[index].GetComponent<IWeapon>();

        if (currentWeapon == null)
        {
            Debug.LogWarning($"Weapon at index {index} does not implement IWeapon interface!");
        }
    }
}
