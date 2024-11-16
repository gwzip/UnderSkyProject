using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectionSystem : MonoBehaviour
{
    public InputAction collectAction; // 수집 키 입력 액션 (인스펙터에서 설정 가능)
    public List<GameObject> collectedItems = new List<GameObject>(); // 수집된 요소를 저장하는 리스트
    private GameObject currentItemInRange; // 현재 플레이어 근처의 수집 가능 요소

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
        // "Collection" 태그를 가진 요소만 감지
        if (other.CompareTag("Collection"))
        {
            currentItemInRange = other.gameObject;
            Debug.Log($"Item discovered: {currentItemInRange.name}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 영역을 벗어나면 현재 수집 가능 요소를 초기화
        if (currentItemInRange != null && other.gameObject == currentItemInRange)
        {
            currentItemInRange = null;
            Debug.Log("Left the item range.");
        }
    }

    void OnCollect(InputAction.CallbackContext context)
    {
        // 수집 가능 상태인지 확인
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
        // 요소를 리스트에 추가하고 로그 출력
        collectedItems.Add(item);
        Debug.Log($"Collected: {item.name}");

        // 수집된 요소를 비활성화 또는 파괴
        item.SetActive(false);

        // 현재 수집 가능한 요소 초기화
        currentItemInRange = null;
    }

    public List<GameObject> GetCollectedItems()
    {
        return collectedItems; // 수집된 요소 리스트 반환
    }
}
