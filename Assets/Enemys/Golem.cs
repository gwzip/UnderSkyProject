using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    public float moveSpeed = 0.5f; // �ſ� ���� �̵� �ӵ�
    public int attackPower = 3; // ���ݷ�
    public int health = 20; // ü��
    public int defense = 15; // ����
    public float attackRange = 3f; // ���� ���� �ݰ�
    public float attackCooldown = 5f; // ���� ��Ÿ��
    public GameObject groundSmashEffect; // �ٴ� ���� ȿ��

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        //�̰� ������ ���� �κ��Դϴپ�
        //foreach (Collider2D hit in hits)
        //{
        //    if (hit.CompareTag("Player"))
        //    {
        //        Player player = hit.GetComponent<Player>();
        //        if (player != null)
        //        {
        //            player.TakeDamage(attackPower);
        //        }
        //    }
        //}

        if (groundSmashEffect != null)
        {
            GameObject effect = Instantiate(groundSmashEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }


    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(0, damage - defense);
        health -= actualDamage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
