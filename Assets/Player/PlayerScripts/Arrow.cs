using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float damage;
    private float range;
    private float speed = 10f; // 화살 속도
    private Vector3 startPosition;
    private bool hasHit; // 충돌 여부를 체크하는 변수

    public void SetProperties(float damage, float range)
    {
        this.damage = damage;
        this.range = range;
        startPosition = transform.position;
    }

    private void Update()
    {
        // 충돌 후에는 이동을 멈춘 상태로 유지
        if (hasHit) return;

        // 화살 이동
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 이동 거리 체크
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject); // 일정 거리 이동 후 파괴
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return; // 이미 충돌한 경우 중복 처리를 방지

        hasHit = true; // 충돌 상태로 설정
        speed = 0f; // 속도 0으로 설정해서 멈추기

        Debug.Log("Arrow hit and stopped at: " + collision.gameObject.name); // 충돌 디버그 로그

        // 충돌 지점에서 위치 고정
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // 리지드바디 속도 제거
            rb.isKinematic = true; // 물리 영향을 받지 않도록 설정
        }
        Invoke("DestroyArrow", 3f);
    }

    private void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
