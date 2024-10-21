using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    public float moveSpeed = 0.5f; // 매우 느린 이동 속도
    public int attackPower = 3; // 공격력
    public int health = 20; // 체력
    public int defense = 15; // 방어력
    public float attackRange = 3f; // 범위 공격 반경
    public float attackCooldown = 5f; // 공격 쿨타임
    public GameObject groundSmashEffect; // 바닥 공격 효과

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

        //이건 데미지 들어가는 부분입니다아
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
