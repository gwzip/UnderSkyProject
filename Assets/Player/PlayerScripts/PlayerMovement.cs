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
        if (inputVec.y > 0)
        {
            spriteRenderer.sprite = backSprite;
        }
        else if (inputVec.y < 0) 
        {
            spriteRenderer.sprite = frontSprite;
        }
        else if (inputVec.x < 0)
        {
            spriteRenderer.sprite = leftSprite;
        }
        else if (inputVec.x > 0)
        {
            spriteRenderer.sprite = rightSprite;
        }
    }
}
