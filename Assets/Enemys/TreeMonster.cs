using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMonster : MonoBehaviour
{
    public float moveSpeed = 0.5f; // 이속
    public int attackPower = 2; // 공격력
    public int health = 30; // 체력
    public int defense = 2; // 방어력
    public float attackRange = 10f; // 원거리 공격 범위
    public float attackCooldown = 2f; // 공격 쿨타임
    public GameObject treeBomb; // 발사체

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown; // 게임 시작 시 바로 공격하지 않긔
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
        // 발사체를 생성하여 플레이어 방향으로 발사
        GameObject projectile = Instantiate(treeBomb, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(0, damage - defense); // 방어력에 따른 실제 피해 계산
        health -= actualDamage;

        if (health <= 0)
        {
            Destroy(gameObject); // 체력이 0 이하가 되면 오브젝트 파괴
        }
    }
}
