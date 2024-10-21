using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown = 1f;

    private bool isDashing;
    private bool canDash = true;
    private float dashTime;
    private float dashCooldownTime;
    private Vector2 dashDirection;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            if (dashTime > 0)
            {
                Vector2 nextVec = dashDirection * dashSpeed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVec);
                dashTime -= Time.fixedDeltaTime;
            }
            else
            {
                isDashing = false;
                StartCooldown();  // 대쉬 종료 시 쿨다운 바로 시작
            }
        }
        else
        {
            Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }

        if (!canDash)
        {
            dashCooldownTime -= Time.fixedDeltaTime;
            if (dashCooldownTime <= 0)
            {
                canDash = true;
            }
        }
    }


    public void OnDash(InputValue value)
    {
        if (canDash && !isDashing && inputVec != Vector2.zero)
        {
            Debug.Log("test");
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashDirection = inputVec;
        canDash = false;
    }

    void StartCooldown()
    {
        dashCooldownTime = dashCooldown;
    }
    public void OnMove(InputValue value)
    {
        if (!isDashing)
        {
            inputVec = value.Get<Vector2>();
        }
    }
}