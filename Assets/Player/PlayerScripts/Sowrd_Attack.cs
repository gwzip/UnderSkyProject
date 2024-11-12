using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword_Attack : MonoBehaviour, IWeapon
{
    public float attackRange = 0.5f;
    public float attackRadius = 0.1f;
    public LayerMask enemyLayer;
    public int attackDamage = 10;
    public float attackCooldown = 0.5f;
    public GameObject attackEffect; // ���� �� ��Ÿ���� ȿ�� ������Ʈ

    public InputAction attackAction; // ���� �Է� �׼�
    private bool canAttack = true;

    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        // ������ �� ���� ȿ�� ��Ȱ��ȭ
        if (attackEffect != null)
        {
            attackEffect.SetActive(false);
        }
    }

    private void OnEnable()
    {
        attackAction.Enable();
        attackAction.performed += OnAttack; // OnAttack �޼��忡 �׼� �Ҵ�
    }

    private void OnDisable()
    {
        attackAction.Disable();
        attackAction.performed -= OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            Use();
        }
    }

    public void Use()
    {
        if (canAttack)
        {
            Vector2 attackDirection = GetFacingDirection();
            if (attackDirection == Vector2.zero)
            {
                Debug.LogWarning("Attack direction is zero. Ensure GetFacingDirection() returns a valid direction.");
                return;
            }
            Attack(attackDirection);
            Debug.Log("Attack!!!");
        }
    }

    Vector2 GetFacingDirection()
    {
        if (playerMovement != null)
        {
            if (playerMovement.inputVec != Vector2.zero)
            {
                return playerMovement.inputVec.normalized;
            }
            return playerMovement.lastDirection; // �Է��� ���� �� ������ ������ ���
        }
        return Vector2.down; // playerMovement�� ���� ��� �⺻������ �Ʒ� ������ ��ȯ
    }

    void Attack(Vector2 attackDirection)
    {
        if (attackDirection != Vector2.zero)
        {
            canAttack = false;
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
                else
                {
                    Debug.LogWarning("Attack direction is not aligned with primary axes. Verify logic.");
                }
            }

            // ���� ȿ�� ������Ʈ Ȱ��ȭ
            if (attackEffect != null)
            {
                attackEffect.SetActive(true);
            }

            StartCoroutine(AttackCooldown());

            // ���� ȿ���� ���� �� ��Ȱ��ȭ
            StartCoroutine(DisableAttackEffect());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator DisableAttackEffect()
    {
        yield return new WaitForSeconds(attackCooldown); // ���� ���� �ð� ���� ���
        if (attackEffect != null)
        {
            attackEffect.SetActive(false); // ������ ������ ��Ȱ��ȭ
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerMovement != null && GetFacingDirection() != Vector2.zero)
        {
            Vector2 attackPosition = (Vector2)transform.position + GetFacingDirection().normalized * attackRange;
            Gizmos.DrawWireSphere(attackPosition, attackRadius);
        }
    }
}
