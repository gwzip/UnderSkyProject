using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName; // ���� �̸�
    public string weaponType; // ���� Ÿ�� (����, ���Ÿ�, ���� ��)
    public int basicDamage; // �⺻ ���� ������
    public int strongDamage; // ������ ������
    public int skillDamage; // ��ų ������
    public float basicCooldown; // �⺻ ���� ��Ÿ��
    public float strongCooldown; // ������ ��Ÿ��
    public float strongCharge; // ������ ���� �ð�
    public float skillCooldown; // ��ų ��Ÿ��
    public float skillCharge; // ��ų ���� �ð�
    public Sprite weaponIcon; // ���� ������ (UI ǥ�ÿ�)
    public string basicDescription; // �⺻ ���� ����
    public string strongDescription; // ������ ����
    public string skillDescription; // ��ų ����
}
