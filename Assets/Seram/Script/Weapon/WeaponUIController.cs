using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUIController : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText; // 무기 이름
    public Image weaponIcon; // 무기 아이콘 이미지

    public void UpdateWeaponUI(string weaponName, Sprite icon = null)
    {
        weaponNameText.text = weaponName;

        // 무기 아이콘 업데이트
        if (weaponIcon != null && icon != null)
        {
            weaponIcon.sprite = icon;
        }
    }

    public void ShowWeaponUI(float duration = 2f)
    {
        gameObject.SetActive(true); // UI 활성화
        CancelInvoke(nameof(HideWeaponUI)); // 이전 타이머 취소
        Invoke(nameof(HideWeaponUI), duration); // 일정 시간 후 UI 숨김
    }

    private void HideWeaponUI()
    {
        gameObject.SetActive(false); // UI 비활성화
    }
}
