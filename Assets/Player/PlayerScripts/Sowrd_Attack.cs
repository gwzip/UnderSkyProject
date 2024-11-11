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
    private bool canAttack = true;

    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
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
        }
        return Vector2.zero;
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
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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
