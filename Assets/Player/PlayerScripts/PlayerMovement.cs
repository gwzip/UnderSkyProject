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
    private SpriteRenderer spriteRenderer;

    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    // Reference to the object that should rotate with the player's direction
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
        }
    }

    void UpdateSpriteDirection()
    {
        if (inputVec != Vector2.zero)
        {
            float angle = Mathf.Atan2(inputVec.y, inputVec.x) * Mathf.Rad2Deg;

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

            // Rotate the object to face the movement direction, correcting for Unity's coordinate system
            if (rotatingObject != null)
            {
                rotatingObject.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }
        }
    }
}
