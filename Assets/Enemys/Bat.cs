using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float followSpeed = 7f; // 플레이어를 따라올 때의 속도
    public float dashSpeed = 15f; // 대쉬 속도
    public int attackPower = 2; // 공격력
    public int health = 15; // 체력
    public float followRange = 30f; // 플레이어를 추적하는 거리
    public float dashRange = 5f; // 대쉬를 시작하는 거리
    private Transform player;
    private Rigidbody2D rb;
    private bool isDashing = false;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어가 대쉬 범위 내에 들어오면 대쉬
        if (distance <= dashRange && !isDashing)
        {
            StartCoroutine(DashTowardsPlayer());
        }
        else if (distance <= followRange)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * followSpeed * Time.deltaTime);
    }

    private IEnumerator DashTowardsPlayer()
    {
        isDashing = true;

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + (direction * dashRange);

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime));
            yield return null;
        }

        //대쉬 후 기다림
        yield return new WaitForSeconds(1f);
        isDashing = false;
    }

    // 이건 대미지 받으면 깎이는건데 아직 안된다고 하니 일단 장식용으로 들어가만 있음
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // 체력이 0 이하가 되면 오브젝트 파괴
        }
    }
}
