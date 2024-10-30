using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoguelikeTemplate
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Temporary")]
        public ClassCard[] classCards;

        [Header("Boss UI")]
        public Transform bossUI;
        public Text bossName;
        public Image bossIcon;
        public MySlider bossHealthSlider;
        public Text bossHealthText;

        [Header("Player name UI")]
        public Transform playerUI;
        public string playerName;
        public GameObject verifyText;
        public Text nameText;
        public InputField nameField;

        [Header("Pools and damage")]
        public GameObject hitNumberPrefab;
        public List<Projectile> arrowPool = new List<Projectile>();
        public List<Projectile> magicPool = new List<Projectile>();

        [Header("Other")]
        public GameObject classScreen;
        public Transform nameScreen;
        public Texture2D cursorSprite;
        public Text countdownText;
        public int coins;


        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
        }

        void Start()
        {
            instance = this;
            DontDestroyOnLoad(this);
            FadePanel.instance.group.alpha = 1;
            FadePanel.instance.Fade();
            Vector2 hotSpot = new Vector2(cursorSprite.width / 8, cursorSprite.height / 10);
            Cursor.SetCursor(cursorSprite, hotSpot, CursorMode.Auto);

            nameText.text = playerName;
        }

        void Update()
        {
            // Select Class from shortcut
            //if (Input.GetKeyDown(KeyCode.U)) classCards[0].GiveStats(); // Knight
            //if (Input.GetKeyDown(KeyCode.I)) classCards[1].GiveStats(); // Archer 
            //if (Input.GetKeyDown(KeyCode.O)) classCards[2].GiveStats(); // Mage
        }

        public float AngleBetweenTwoPoints(Vector2 from, Vector2 to)
        {
            Vector2 direction = to - from;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public void SetName()
        {
            string name = nameField.text;
            nameField.text = nameField.text.Replace(" ", "");

            if (name != nameField.text) return;

            // Verify name
            if (nameField.text.Length < 3)
            {
                verifyText.SetActive(true);
                CancelInvoke("HideVerification");
                Invoke("HideVerification", 5);
            }
            else
            {
                DialogueManager.instance.names[0] = playerName;
                playerName = nameField.text;
                nameText.text = playerName;
                nameScreen.GetComponentInChildren<Button>().interactable = false;
                StartCoroutine(HideNamePrompt());
            }
        }

        IEnumerator HideNamePrompt()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            while (nameScreen.transform.localPosition.y > -400)
            {
                nameScreen.transform.localPosition -= Vector3.up * 20;
                yield return waitForFixedUpdate;
            }

            nameScreen.gameObject.SetActive(false);
            string[] phrases = new string[] { "1Nice to meet you " + GameManager.instance.playerName + "!",
                "1You will find the Guild Master in front of the dungeon. Speak to him before entering.",
                "1Good Luck!" };
            DialogueManager.instance.AddPhrases(phrases);
        }

        IEnumerator ShowNamePrompt()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            while (nameScreen.transform.localPosition.y < 0)
            {
                nameScreen.transform.localPosition += Vector3.up * 20;
                yield return waitForFixedUpdate;
            }

            nameScreen.transform.localPosition = Vector3.zero;
        }

        IEnumerator ShowClassPrompt()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            while (classScreen.transform.localPosition.x < 0)
            {
                classScreen.transform.localPosition += Vector3.right * 100;
                yield return waitForFixedUpdate;
            }

            classScreen.transform.localPosition = Vector3.zero;
        }

        public void PlayerUIPrompt(bool show) => StartCoroutine(HealthbarCO(show, playerUI));

        IEnumerator HealthbarCO(bool show, Transform ui)
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            if (show)
            {
                while (ui.localPosition.y > 600)
                {
                    ui.localPosition -= Vector3.up * 20;
                    yield return waitForFixedUpdate;
                }

                ui.localPosition = Vector3.up * 600;
            }
            else
            {
                while (ui.localPosition.y < 900)
                {
                    ui.localPosition += Vector3.up * 20;
                    yield return waitForFixedUpdate;
                }
            }
        }

        public void PromptName()
        {
            nameScreen.gameObject.SetActive(true);
            var system = EventSystem.current;
            system.SetSelectedGameObject(nameField.gameObject);
            StartCoroutine(ShowNamePrompt());
        }

        public void PromptClass()
        {
            classScreen.SetActive(true);
            classScreen.transform.GetChild(0).gameObject.SetActive(true); // Enable Text
            classScreen.transform.GetChild(0).localPosition = Vector3.zero; // Reset Text Position
            classScreen.transform.localPosition = Vector3.right * -2000;
            for (int i = 0; i < classCards.Length; i++) classCards[i].ResetUI();

            StartCoroutine(ShowClassPrompt());
        }

        public void ShowPlayerUI()
        {
            playerUI.gameObject.SetActive(true);
            StartCoroutine(HealthbarCO(true, playerUI));
            Player.instance.canMove = true;
        }

        public void ShowBossUI(bool show)
        {
            StartCoroutine(HealthbarCO(show, bossUI));
        }

        private void HideVerification() => verifyText.SetActive(false); // Invoked

        private static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}
