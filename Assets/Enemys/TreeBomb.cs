using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBomb : MonoBehaviour
{
    public int damage = 2; // �߻�ü�� ���ط�
    public float lifetime = 5f; // �߻�ü�� ���� �ֱ�

    void Start()
    {
        // ���� �ð��� ������ ������Ʈ ����
        Destroy(gameObject, lifetime);
    }

    // ������Ʈ�� ī�޶� ������ ������ ����
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // �÷��̾�� �浹 �� ������Ʈ ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
