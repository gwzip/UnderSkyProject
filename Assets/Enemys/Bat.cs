using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float followSpeed = 7f; // �÷��̾ ����� ���� �ӵ�
    public float dashSpeed = 15f; // �뽬 �ӵ�
    public int attackPower = 2; // ���ݷ�
    public int health = 15; // ü��
    public float followRange = 30f; // �÷��̾ �����ϴ� �Ÿ�
    public float dashRange = 5f; // �뽬�� �����ϴ� �Ÿ�
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

        // �÷��̾ �뽬 ���� ���� ������ �뽬
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

        //�뽬 �� ��ٸ�
        yield return new WaitForSeconds(1f);
        isDashing = false;
    }

    // �̰� ����� ������ ���̴°ǵ� ���� �ȵȴٰ� �ϴ� �ϴ� ��Ŀ����� ���� ����
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // ü���� 0 ���ϰ� �Ǹ� ������Ʈ �ı�
        }
    }
}
