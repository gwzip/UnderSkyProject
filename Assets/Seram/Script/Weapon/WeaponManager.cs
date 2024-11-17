using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weaponObjects; // ���� ������ �迭
    private int currentWeaponIndex = 0; // ���� ���� �ε���

    void Start()
    {
        // ���� ���⸦ �Ѽհ����� ����
        SetStartingWeapon("�Ѽհ�");
        UpdateWeaponVisibility();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // EŰ�� ���� ��ü
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
