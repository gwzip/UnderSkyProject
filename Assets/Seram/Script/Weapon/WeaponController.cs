using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    private float lastBasicAttackTime;
    private float lastStrongAttackTime;
    private float lastSkillTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ��Ŭ��: �⺻ ����
        {
            PerformBasicAttack();
        }
        else if (Input.GetMouseButtonDown(1)) // ��Ŭ��: ������
        {
            PerformStrongAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // QŰ: ��ų
        {
            PerformSkill();
        }
    }

    void PerformBasicAttack()
    {
        if (Time.time >= lastBasicAttackTime + weaponData.basicCooldown)
        {
            lastBasicAttackTime = Time.time;
            Debug.Log($"{weaponData.weaponName} �⺻ ����! ������: {weaponData.basicDamage}");
            // �߰� ���� ���� (�ִϸ��̼�, ������ ó�� ��)
        }
    }

    void PerformStrongAttack()
    {
        if (Time.time >= lastStrongAttackTime + weaponData.strongCooldown)
        {
            lastStrongAttackTime = Time.time;
            Debug.Log($"{weaponData.weaponName} ������! ������: {weaponData.strongDamage}");
            // �߰� ������ ����
        }
    }

    void PerformSkill()
    {
        if (Time.time >= lastSkillTime + weaponData.skillCooldown)
        {
            lastSkillTime = Time.time;
            Debug.Log($"{weaponData.weaponName} ��ų ���! ������: {weaponData.skillDamage}");
            // �߰� ��ų ����
        }
    }
}
