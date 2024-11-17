using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectionIntotheBook : MonoBehaviour
{
    public InputAction openBookAction; // 도감 열기/닫기 키 입력 액션
    public InputAction cancelAction; // 도감 닫기 (취소) 키 입력 액션
    public GameObject collectionUI; // 도감 UI 패널
    public Transform collectionListContainer; // 도감 UI 항목이 추가될 부모 오브젝트
    public GameObject collectionEntryPrefab; // 도감 항목 UI 프리팹 (이미지 포함)
    public WeaponPickupSystem weaponPickupSystem; // WeaponPickupSystem 참조

    private bool isBookOpen = false; // 도감 UI가 열려 있는지 상태 확인
    private HashSet<GameObject> displayedWeapons = new HashSet<GameObject>(); // 이미 도감에 표시된 무기

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

        if (weaponPickupSystem == null)
        {
            Debug.LogError("WeaponPickupSystem reference is missing!");
        }
    }

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

        // WeaponPickupSystem의 수집된 무기 리스트를 가져와 도감 UI 갱신
        UpdateCollectionFromWeaponPickup();

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

    void UpdateCollectionFromWeaponPickup()
    {
        if (weaponPickupSystem == null) return;

        List<GameObject> collectedWeapons = weaponPickupSystem.GetCollectedWeapons();

        foreach (GameObject weapon in collectedWeapons)
        {
            if (displayedWeapons.Contains(weapon)) continue;

            // 새로운 UI 항목 생성
            GameObject newEntry = Instantiate(collectionEntryPrefab, collectionListContainer);
            var imageComponent = newEntry.GetComponentInChildren<UnityEngine.UI.Image>();
            if (imageComponent != null)
            {
                var spriteRenderer = weapon.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    imageComponent.sprite = spriteRenderer.sprite; // 무기 스프라이트 가져오기
                }
            }

            displayedWeapons.Add(weapon);
        }
    }
}
