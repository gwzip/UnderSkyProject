using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectionIntotheBook : MonoBehaviour
{
    public InputAction openBookAction; // ���� ����/�ݱ� Ű �Է� �׼�
    public InputAction cancelAction; // ���� �ݱ� (���) Ű �Է� �׼�
    public GameObject collectionUI; // ���� UI �г�
    public Transform collectionListContainer; // ���� UI �׸��� �߰��� �θ� ������Ʈ
    public GameObject collectionEntryPrefab; // ���� �׸� UI ������ (�̹��� ����)
    public WeaponPickupSystem weaponPickupSystem; // WeaponPickupSystem ����

    private bool isBookOpen = false; // ���� UI�� ���� �ִ��� ���� Ȯ��
    private HashSet<GameObject> displayedWeapons = new HashSet<GameObject>(); // �̹� ������ ǥ�õ� ����

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
        // �ʱ⿡�� ���� UI�� ����
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

        // WeaponPickupSystem�� ������ ���� ����Ʈ�� ������ ���� UI ����
        UpdateCollectionFromWeaponPickup();

        // ���� UI�� ���� �ð��� ����
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

        // ���� UI�� ������ �ð��� �ٽ� �帣�� ��
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

            // ���ο� UI �׸� ����
            GameObject newEntry = Instantiate(collectionEntryPrefab, collectionListContainer);
            var imageComponent = newEntry.GetComponentInChildren<UnityEngine.UI.Image>();
            if (imageComponent != null)
            {
                var spriteRenderer = weapon.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    imageComponent.sprite = spriteRenderer.sprite; // ���� ��������Ʈ ��������
                }
            }

            displayedWeapons.Add(weapon);
        }
    }
}
