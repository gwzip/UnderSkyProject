using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBomb : MonoBehaviour
{
    public int damage = 2; // 발사체의 피해량
    public float lifetime = 5f; // 발사체의 생명 주기

    void Start()
    {
        // 일정 시간이 지나면 오브젝트 삭제
        Destroy(gameObject, lifetime);
    }

    // 오브젝트가 카메라 밖으로 나가면 삭제
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // 플레이어와 충돌 시 오브젝트 삭제
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
