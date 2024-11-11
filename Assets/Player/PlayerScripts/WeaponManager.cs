using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weapons; // 무기 오브젝트 리스트
    private int currentWeaponIndex = 0;
    private IWeapon currentWeapon;

    // InputAction 변수 선언
    public InputActionAsset inputActions;
    private InputAction changeWeaponAction;
    private InputAction attackAction;

    private void OnEnable()
    {
        // InputAction 활성화
        changeWeaponAction.Enable();
        attackAction.Enable();
    }

    private void OnDisable()
    {
        // InputAction 비활성화
        changeWeaponAction.Disable();
        attackAction.Disable();
    }

    private void Start()
    {
        // InputActionAsset에서 액션 가져오기
        var actionMap = inputActions.FindActionMap("Player"); // 'Player'라는 Action Map을 찾아서 사용
        changeWeaponAction = actionMap.FindAction("Change");
        attackAction = actionMap.FindAction("Attack");

        // 초기 무기 설정
        EquipWeapon(currentWeaponIndex);

        // InputAction에 이벤트 등록
        changeWeaponAction.performed += _ => ChangeWeapon();
        attackAction.performed += _ => UseWeapon();
    }

    private void ChangeWeapon()
    {
        // 현재 무기 비활성화
        weapons[currentWeaponIndex].SetActive(false);

        // 다음 무기로 인덱스 변경
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;

        // 새로운 무기 장착
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
