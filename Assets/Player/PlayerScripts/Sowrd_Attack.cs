using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword_Attack : MonoBehaviour
{
    public float attackRange = 1f;       // ���� ����
    public float attackRadius = 0.5f;    // ���� ���� ������
    public LayerMask enemyLayer;         // �� ���̾� ����
    public int attackDamage = 10;        // ���� ������
    public float attackCooldown = 0.5f;  // ���� ��ٿ� �ð�
    private bool canAttack = true;       // ���� ���� ����

    private PlayerMovement playerMovement;  // PlayerMovement ��ũ��Ʈ ����
    private Animator animator;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            Debug.Log("Attack!!!");
            Attack();
        }
    }


    void Attack()
    {
        Vector2 attackDirection = playerMovement.inputVec;  // �̵� ������ ������� ���� ���� ����

        if (attackDirection != Vector2.zero)
        {
            canAttack = false;  // ���� �߿��� �߰� ������ ���� �ʵ��� ����

            // ������ ��ġ ��� (���� ��ġ���� ���� �������� ���� �Ÿ���ŭ ������ ��)
            Vector2 attackPosition = (Vector2)transform.position + attackDirection.normalized * attackRange;

            // ���� ���� ������ ���� (���� ������ ���� ������ ����)
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRadius, enemyLayer);

            // ���� ���� �ִ� ��� ���鿡�� ������ �ֱ�
            //foreach (Collider2D enemy in hitEnemies)
            //{
            //    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            //}

            // ���� �ִϸ��̼� ���
            if (animator != null)
            {
                if (attackDirection == Vector2.up)
                {
                    animator.Play("Attack_Up");
                }
                else if (attackDirection == Vector2.down)
                {
                    animator.Play("Attack_Down");
                }
                else if (attackDirection == Vector2.left)
                {
                    animator.Play("Attack_Left");
                }
                else if (attackDirection == Vector2.right)
                {
                    animator.Play("Attack_Right");
                }
            }

            // ���� �ð� �� �ٽ� ���� �����ϵ��� ����
            StartCoroutine(AttackCooldown());
        }
    }

    // ���� ��ٿ� �ð� ����
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // ���� ���� �ð�ȭ�� ���� ����׿� �Լ�
    private void OnDrawGizmosSelected()
    {
        if (playerMovement != null && playerMovement.inputVec != Vector2.zero)
        {
            Vector2 attackPosition = (Vector2)transform.position + playerMovement.inputVec.normalized * attackRange;
            Gizmos.DrawWireSphere(attackPosition, attackRadius);
        }
    }
}
