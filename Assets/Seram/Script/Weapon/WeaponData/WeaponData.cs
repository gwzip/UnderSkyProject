using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName; // 무기 이름
    public string weaponType; // 무기 타입 (근접, 원거리, 마법 등)
    public int basicDamage; // 기본 공격 데미지
    public int strongDamage; // 강공격 데미지
    public int skillDamage; // 스킬 데미지
    public float basicCooldown; // 기본 공격 쿨타임
    public float strongCooldown; // 강공격 쿨타임
    public float strongCharge; // 강공격 차지 시간
    public float skillCooldown; // 스킬 쿨타임
    public float skillCharge; // 스킬 차지 시간
    public Sprite weaponIcon; // 무기 아이콘 (UI 표시용)
    public string basicDescription; // 기본 공격 설명
    public string strongDescription; // 강공격 설명
    public string skillDescription; // 스킬 설명
}
