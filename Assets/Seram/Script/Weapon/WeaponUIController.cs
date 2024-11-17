using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUIController : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText; // ���� �̸�
    public Image weaponIcon; // ���� ������ �̹���

    public void UpdateWeaponUI(string weaponName, Sprite icon = null)
    {
        weaponNameText.text = weaponName;

        // ���� ������ ������Ʈ
        if (weaponIcon != null && icon != null)
        {
            weaponIcon.sprite = icon;
        }
    }

    public void ShowWeaponUI(float duration = 2f)
    {
        gameObject.SetActive(true); // UI Ȱ��ȭ
        CancelInvoke(nameof(HideWeaponUI)); // ���� Ÿ�̸� ���
        Invoke(nameof(HideWeaponUI), duration); // ���� �ð� �� UI ����
    }

    private void HideWeaponUI()
    {
        gameObject.SetActive(false); // UI ��Ȱ��ȭ
    }
}
