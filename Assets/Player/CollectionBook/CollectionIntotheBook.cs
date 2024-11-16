using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectionIntotheBook : MonoBehaviour
{
    public InputAction openBookAction; // ���� ����/�ݱ� Ű �Է� �׼�
    public InputAction cancelAction; // ���� �ݱ� (���) Ű �Է� �׼�
    public GameObject collectionUI; // ���� UI �г�
    //public List<Sprite> collectedItemSprites = new List<Sprite>(); // ������ �������� ��������Ʈ ����Ʈ
    //public Transform collectionListContainer; // ���� UI �׸��� �߰��� �θ� ������Ʈ
    //public GameObject collectionEntryPrefab; // ���� �׸� UI ������ (�̹��� ����)

    private bool isBookOpen = false; // ���� UI�� ���� �ִ��� ���� Ȯ��

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

    //    // ���� UI�� ���ο� �׸� �߰�
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
}
