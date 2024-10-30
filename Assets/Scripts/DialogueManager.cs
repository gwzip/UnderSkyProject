using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RoguelikeTemplate
{

    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;

        public GameObject chatBox;
        public Image icon;
        public Text nameText;
        public Text chatText;
        public int currentPhrase;
        public List<string> phrases;
        public Sprite[] icons;
        public string[] names;
        public delegate void DialogueEnd();
        public DialogueEnd dialogueEnd;

        private AudioSource audioSource;
        [HideInInspector] public bool UIactive;
        private bool writingText;

        private void Start()
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            // If chatbox is active and displayed
            if (chatBox.activeInHierarchy && chatBox.transform.localPosition.y > -400)
            {
                // Skip using space, mouse click or enter
                if (Input.GetButtonUp("Jump") || Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.KeypadEnter))
                {
                    // If still writing text, complete it
                    if (writingText)
                    {
                        //if (runningCoroutine != null) StopCoroutine(runningCoroutine);
                        StopAllCoroutines();
                        chatText.text = phrases[currentPhrase];
                        audioSource.Stop();
                        writingText = false;
                    } // else go to the next phrase
                    else DisplayText();
                }
            }
        }

        public void SetDelegate(string delegateName)
        {
            switch (delegateName)
            {
                case "SetName": dialogueEnd = GameManager.instance.PromptName; break;
                case "PromptClass": dialogueEnd = GameManager.instance.PromptClass; break;
                case "ShowUI": dialogueEnd = GameManager.instance.ShowPlayerUI; break;
            }
        }

        public void DisplayText()
        {
            // If currentPhrase is -1, activate chat
            if (currentPhrase == -1) PromptChat(true);
            currentPhrase++;

            if (currentPhrase == phrases.Count) // If last phrase 
            {
                if (dialogueEnd == null) Player.instance.Immobilize(false); // Let player go if there are no delegates to be called
                else // Else call the delegate
                {
                    dialogueEnd();
                    dialogueEnd = null;
                }
                PromptChat(false); // Hide ChatBox

                return; 
            }

            StartCoroutine(ChatboxTextType(phrases[currentPhrase])); // Tyepwriter effect
        }

        public IEnumerator ChatboxTextType(string phrase)
        {
            audioSource.Play();
            chatText.text = "";
            List<char> phraseChars = new List<char>(phrase.ToCharArray()); 
            writingText = true;

            // Select icon
            int index = int.Parse(phraseChars[0].ToString());
            icon.sprite = icons[index];
            nameText.text = names[index];
            phraseChars.RemoveAt(0);
            phrases[currentPhrase] = new string(phraseChars.ToArray());

            for (int i = 0; i < phraseChars.Count; i++)
            {
                if (!writingText)
                {
                    chatText.text = phrase;
                    break;
                }
                chatText.text += phraseChars[i];

                yield return new WaitForSeconds(.035f);
            }

            audioSource.Stop();
            writingText = false;
        }

        public void AddPhrase(string phrase) // Add a single phrase and activate chat
        {
            phrases.Clear();
            chatText.text = string.Empty;
            currentPhrase = -1;
            phrases.Add(phrase);
            PromptChat(true);
        }

        public void AddPhrases(string[] phrase) // Add phrase range and activate chat
        {
            phrases.Clear();
            chatText.text = string.Empty;
            currentPhrase = -1;
            phrases.AddRange(phrase);
            PromptChat(true);
        }

        public void PromptChat(bool prompt)
        {
            // If chat is already active don't bother
            if (prompt) // Update chat icon and text, and Disable player UI
            {
                int index = int.Parse(phrases[0].ToCharArray()[0].ToString());
                icon.sprite = icons[index];
                nameText.text = names[index];
                if (GameManager.instance.playerUI.gameObject.activeInHierarchy) GameManager.instance.PlayerUIPrompt(false);
            }
            else
            {
                if (UIactive) GameManager.instance.PlayerUIPrompt(true); // Enable Player UI
            }

            StartCoroutine(PromptChatCO(prompt)); // Smooth chat Show/Hide
        }

        private IEnumerator PromptChatCO(bool prompt) // Smooth chat Show/Hide
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

            if (prompt)
            {
                // If chat is already active don't bother
                if (chatBox.activeInHierarchy) yield break;

                chatBox.SetActive(true);

                while (chatBox.transform.localPosition.y < -370)
                {
                    chatBox.transform.localPosition += Vector3.up * 25;
                    yield return waitForFixedUpdate;
                }

                DisplayText();
                yield break;
            }

            if (!chatBox.activeInHierarchy) yield break;

            while (chatBox.transform.localPosition.y > -940)
            {
                chatBox.transform.localPosition -= Vector3.up * 25;
                yield return waitForFixedUpdate;
            }
            chatBox.SetActive(false);
        }
    }
}
