using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public Vector2 lastDirection = Vector2.down; // 마지막 방향, 기본은 아래쪽
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
    private SpriteRenderer spriteRenderer;

    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    // 회전할 대상 오브젝트
    public Transform rotatingObject;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                StartCooldown();
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

        UpdateSpriteDirection();
    }

    public void OnDash(InputValue value)
    {
        if (canDash && !isDashing && inputVec != Vector2.zero)
        {
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

            // 입력 벡터가 0이 아닌 경우 마지막 방향 업데이트
            if (inputVec != Vector2.zero)
            {
                lastDirection = inputVec.normalized;
            }
        }
    }

    void UpdateSpriteDirection()
    {
        Vector2 direction = inputVec != Vector2.zero ? inputVec : lastDirection;

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle >= -45f && angle < 45f)
            {
                spriteRenderer.sprite = rightSprite;
            }
            else if (angle >= 45f && angle < 135f)
            {
                spriteRenderer.sprite = backSprite;
            }
            else if (angle >= -135f && angle < -45f)
            {
                spriteRenderer.sprite = frontSprite;
            }
            else
            {
                spriteRenderer.sprite = leftSprite;
            }

            // 회전할 대상 오브젝트를 이동 방향으로 회전시킴
            if (rotatingObject != null)
            {
                rotatingObject.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }
        }
    }
}
