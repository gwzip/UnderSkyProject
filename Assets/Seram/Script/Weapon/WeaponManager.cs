using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weaponObjects; // 무기 프리팹 배열
    private int currentWeaponIndex = 0; // 현재 무기 인덱스

    void Start()
    {
        // 시작 무기를 한손검으로 설정
        SetStartingWeapon("한손검");
        UpdateWeaponVisibility();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E키로 무기 교체
        {
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponObjects.Length;
        UpdateWeaponVisibility();
    }

    void UpdateWeaponVisibility()
    {
        for (int i = 0; i < weaponObjects.Length; i++)
        {
            weaponObjects[i].SetActive(i == currentWeaponIndex);
        }
    }

    void SetStartingWeapon(string weaponName)
    {
        for (int i = 0; i < weaponObjects.Length; i++)
        {
            var weaponController = weaponObjects[i].GetComponent<WeaponController>();
            if (weaponController != null && weaponController.weaponData.weaponName == weaponName)
            {
                currentWeaponIndex = i; 
                return;
            }
        }
    }
}
