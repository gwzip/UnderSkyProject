using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectionIntotheBook : MonoBehaviour
{
    public InputAction openBookAction; // 도감 열기/닫기 키 입력 액션
    public InputAction cancelAction; // 도감 닫기 (취소) 키 입력 액션
    public GameObject collectionUI; // 도감 UI 패널
    //public List<Sprite> collectedItemSprites = new List<Sprite>(); // 수집된 아이템의 스프라이트 리스트
    //public Transform collectionListContainer; // 도감 UI 항목이 추가될 부모 오브젝트
    //public GameObject collectionEntryPrefab; // 도감 항목 UI 프리팹 (이미지 포함)

    private bool isBookOpen = false; // 도감 UI가 열려 있는지 상태 확인

    void OnEnable()
    {
        openBookAction.Enable();
        cancelAction.Enable();
        openBookAction.performed += ToggleBook;
        cancelAction.performed += CloseBook;
    }

    void OnDisable()
    {
        openBookAction.performed -= ToggleBook;
        cancelAction.performed -= CloseBook;
        openBookAction.Disable();
        cancelAction.Disable();
    }

    void Start()
    {
        // 초기에는 도감 UI를 숨김
        if (collectionUI != null)
        {
            collectionUI.SetActive(false);
        }
    }

    //public void AddToCollection(Sprite itemSprite)
    //{
    //    if (collectedItemSprites.Contains(itemSprite))
    //    {
    //        Debug.Log("Item already in collection.");
    //        return;
    //    }

    //    collectedItemSprites.Add(itemSprite);
    //    Debug.Log("Item added to collection.");

    //    // 도감 UI에 새로운 항목 추가
    //    if (collectionEntryPrefab != null && collectionListContainer != null)
    //    {
    //        GameObject newEntry = Instantiate(collectionEntryPrefab, collectionListContainer);
    //        var imageComponent = newEntry.GetComponentInChildren<UnityEngine.UI.Image>();
    //        if (imageComponent != null)
    //        {
    //            imageComponent.sprite = itemSprite;
    //        }
    //    }
    //}

    void ToggleBook(InputAction.CallbackContext context)
    {
        if (isBookOpen)
        {
            CloseBook(context);
        }
        else
        {
            OpenBook();
        }
    }

    void OpenBook()
    {
        isBookOpen = true;

        if (collectionUI != null)
        {
            collectionUI.SetActive(true);
        }

        // 도감 UI를 열면 시간을 멈춤
        Time.timeScale = 0f;
        Debug.Log("Book opened. Time paused.");
    }

    void CloseBook(InputAction.CallbackContext context)
    {
        if (isBookOpen)
        {
            CloseBook();
        }
    }

    void CloseBook()
    {
        isBookOpen = false;

        if (collectionUI != null)
        {
            collectionUI.SetActive(false);
        }

        // 도감 UI를 닫으면 시간을 다시 흐르게 함
        Time.timeScale = 1f;
        Debug.Log("Book closed. Time resumed.");
    }
}
