using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMonster : MonoBehaviour
{
    public float moveSpeed = 0.5f; // �̼�
    public int attackPower = 2; // ���ݷ�
    public int health = 30; // ü��
    public int defense = 2; // ����
    public float attackRange = 10f; // ���Ÿ� ���� ����
    public float attackCooldown = 2f; // ���� ��Ÿ��
    public GameObject treeBomb; // �߻�ü

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown; // ���� ���� �� �ٷ� �������� �ʱ�
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);


        if (distance <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time; 
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized; 
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime); 
    }

    private void AttackPlayer()
    {
        // �߻�ü�� �����Ͽ� �÷��̾� �������� �߻�
        GameObject projectile = Instantiate(treeBomb, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(0, damage - defense); // ���¿� ���� ���� ���� ���
        health -= actualDamage;

        if (health <= 0)
        {
            Destroy(gameObject); // ü���� 0 ���ϰ� �Ǹ� ������Ʈ �ı�
        }
    }
}
