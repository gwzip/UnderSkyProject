using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword_Attack : MonoBehaviour
{
    public float attackRange = 1f;       // 공격 범위
    public float attackRadius = 0.5f;    // 공격 범위 반지름
    public LayerMask enemyLayer;         // 적 레이어 지정
    public int attackDamage = 10;        // 공격 데미지
    public float attackCooldown = 0.5f;  // 공격 쿨다운 시간
    private bool canAttack = true;       // 공격 가능 여부

    private PlayerMovement playerMovement;  // PlayerMovement 스크립트 참조
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
        Vector2 attackDirection = playerMovement.inputVec;  // 이동 방향을 기반으로 공격 방향 설정

        if (attackDirection != Vector2.zero)
        {
            canAttack = false;  // 공격 중에는 추가 공격을 하지 않도록 설정

            // 공격할 위치 계산 (현재 위치에서 공격 방향으로 일정 거리만큼 떨어진 곳)
            Vector2 attackPosition = (Vector2)transform.position + attackDirection.normalized * attackRange;

            // 범위 내의 적들을 검출 (원형 범위로 공격 범위를 설정)
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRadius, enemyLayer);

            // 범위 내에 있는 모든 적들에게 데미지 주기
            //foreach (Collider2D enemy in hitEnemies)
            //{
            //    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            //}

            // 공격 애니메이션 재생
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

            // 일정 시간 후 다시 공격 가능하도록 설정
            StartCoroutine(AttackCooldown());
        }
    }

    // 공격 쿨다운 시간 설정
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // 공격 범위 시각화를 위한 디버그용 함수
    private void OnDrawGizmosSelected()
    {
        if (playerMovement != null && playerMovement.inputVec != Vector2.zero)
        {
            Vector2 attackPosition = (Vector2)transform.position + playerMovement.inputVec.normalized * attackRange;
            Gizmos.DrawWireSphere(attackPosition, attackRadius);
        }
    }
}
