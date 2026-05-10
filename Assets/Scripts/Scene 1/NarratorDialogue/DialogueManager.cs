using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Wolf")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip wolfHowl;
    [SerializeField] private GameObject wolf;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Post Puzzle")]
    [SerializeField] private TextAsset postPuzzleInkJSON;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;
    private static DialogueManager instance;
    private bool dialogueLocked = false;

    // TAGS
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string PUZZLE_TAG = "activate_puzzle";

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;

        if (canContinueToNextLine
            && currentStory.currentChoices.Count == 0
            && InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (dialogueLocked)
        {
            Debug.Log("Dialogue is locked during puzzle.");
            return;
        }

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // Reset UI
        displayNameText.text = "???";
        portraitAnimator.Play("Narrator");
        layoutAnimator.Play("right");

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            string nextLine = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            CSE_NPCWander npcScript = npc.GetComponent<CSE_NPCWander>();

            foreach (string tag in currentStory.currentTags)
            {
                if (tag.Trim() == "runaway" && npcScript != null)
                {
                    npcScript.RunAway();
                }

                if (tag.Trim() == "wolf_howl")
                {
                    StartCoroutine(FadeInHowl());
                }
            }

            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator FadeInHowl()
    {
        audioSource.clip = wolfHowl;
        audioSource.volume = 0f;
        audioSource.Play();

        float duration = 3f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }

        audioSource.volume = 1f;
    }

    private IEnumerator WolfEvent()
    {
        audioSource.PlayOneShot(wolfHowl);
        yield return new WaitForSeconds(10f);
        wolf.SetActive(true);
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        continueIcon.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueText.text = line;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>') isAddingRichTextTag = false;
                dialogueText.text += letter;
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        continueIcon.SetActive(true);
        DisplayChoices();
        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
            {
                Debug.LogWarning("Tags are not in correct format: " + tag);
                continue;
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;

                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;

                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;

                case PUZZLE_TAG:
                    LoadPuzzleScene(tagValue);
                    break;

                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void LoadPuzzleScene(string sceneName)
    {
        dialogueLocked = true;
        StartCoroutine(ExitDialogueMode());

        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Debug.Log("Loaded puzzle scene: " + sceneName);
        }
        else
        {
            Debug.LogWarning("Scene already loaded: " + sceneName);
        }
    }

    // Called by WinScript when puzzle is completed
    public void UnlockDialogue()
    {
        dialogueLocked = false;

        if (postPuzzleInkJSON != null)
        {
            currentStory = new Story(postPuzzleInkJSON.text);
            currentStory.ChoosePathString("siren_after_puzzle"); // <- jumps to the right knot

            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            displayNameText.text = "???";
            portraitAnimator.Play("Narrator");
            layoutAnimator.Play("right");

            ContinueStory();
        }
        else
        {
            Debug.LogWarning("No post puzzle Ink file assigned!");
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }
}