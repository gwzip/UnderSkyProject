using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    private float lastBasicAttackTime;
    private float lastStrongAttackTime;
    private float lastSkillTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭: 기본 공격
        {
            PerformBasicAttack();
        }
        else if (Input.GetMouseButtonDown(1)) // 우클릭: 강공격
        {
            PerformStrongAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Q키: 스킬
        {
            PerformSkill();
        }
    }

    void PerformBasicAttack()
    {
        if (Time.time >= lastBasicAttackTime + weaponData.basicCooldown)
        {
            lastBasicAttackTime = Time.time;
            Debug.Log($"{weaponData.weaponName} 기본 공격! 데미지: {weaponData.basicDamage}");
            // 추가 공격 로직 (애니메이션, 데미지 처리 등)
        }
    }

    void PerformStrongAttack()
    {
        if (Time.time >= lastStrongAttackTime + weaponData.strongCooldown)
        {
            lastStrongAttackTime = Time.time;
            Debug.Log($"{weaponData.weaponName} 강공격! 데미지: {weaponData.strongDamage}");
            // 추가 강공격 로직
        }
    }

    void PerformSkill()
    {
        if (Time.time >= lastSkillTime + weaponData.skillCooldown)
        {
            lastSkillTime = Time.time;
            Debug.Log($"{weaponData.weaponName} 스킬 사용! 데미지: {weaponData.skillDamage}");
            // 추가 스킬 로직
        }
    }
}
