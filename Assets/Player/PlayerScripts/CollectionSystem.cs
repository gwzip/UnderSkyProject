using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectionSystem : MonoBehaviour
{
    public InputAction collectAction; // ���� Ű �Է� �׼� (�ν����Ϳ��� ���� ����)
    public List<GameObject> collectedItems = new List<GameObject>(); // ������ ��Ҹ� �����ϴ� ����Ʈ
    private GameObject currentItemInRange; // ���� �÷��̾� ��ó�� ���� ���� ���

    void OnEnable()
    {
        collectAction.Enable();
        collectAction.performed += OnCollect;
    }

    void OnDisable()
    {
        collectAction.performed -= OnCollect;
        collectAction.Disable();
    }

    void Start()
    {
        Debug.Log("Collection system initialized.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // "Collection" �±׸� ���� ��Ҹ� ����
        if (other.CompareTag("Collection"))
        {
            currentItemInRange = other.gameObject;
            Debug.Log($"Item discovered: {currentItemInRange.name}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // ������ ����� ���� ���� ���� ��Ҹ� �ʱ�ȭ
        if (currentItemInRange != null && other.gameObject == currentItemInRange)
        {
            currentItemInRange = null;
            Debug.Log("Left the item range.");
        }
    }

    void OnCollect(InputAction.CallbackContext context)
    {
        // ���� ���� �������� Ȯ��
        if (currentItemInRange != null)
        {
            CollectItem(currentItemInRange);
        }
        else
        {
            Debug.Log("No collectible item in range.");
        }
    }

    void CollectItem(GameObject item)
    {
        // ��Ҹ� ����Ʈ�� �߰��ϰ� �α� ���
        collectedItems.Add(item);
        Debug.Log($"Collected: {item.name}");

        // ������ ��Ҹ� ��Ȱ��ȭ �Ǵ� �ı�
        item.SetActive(false);

        // ���� ���� ������ ��� �ʱ�ȭ
        currentItemInRange = null;
    }

    public List<GameObject> GetCollectedItems()
    {
        return collectedItems; // ������ ��� ����Ʈ ��ȯ
    }
}
