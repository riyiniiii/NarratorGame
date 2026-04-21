using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;


    [Header(("Dialogue UI"))]

    [SerializeField] private GameObject dialoguePanel;
    
    [SerializeField] private GameObject continueIcon;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")] 
    
    [SerializeField] private Animator portraitAnimator;

    private Animator layoutAnimator;
    
    [SerializeField] private GameObject[] choices;
    
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;
        
    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    
    private const string PORTRAIT_TAG = "portrait";
    
    private const string LAYOUT_TAG = "layout";
    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }

        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        
        //get the layout animator
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        
        // get all of the choice text
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
        Debug.Log(InputManager.GetInstance());
        
        if (!dialogueIsPlaying)
        {
            return;
        }
        
        //handle continuing to the next line in the dialogue when submit is pressed

        if (canContinueToNextLine
            && currentStory.currentChoices.Count == 0
            && InputManager.GetInstance().GetSubmitPressed())
        {
                ContinueStory();
            
        }
    }
    

    public void EnterDialogueMode(TextAsset inkJSON)
        {
            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            
            //RESET portrait, layouts, and speaker
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
                //set text for the current dialogue line
                if (displayLineCoroutine != null)
                {
                    StopCoroutine(displayLineCoroutine);
                }
                displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
             
                //HANDLE TAGS
                HandleTags(currentStory.currentTags);
            }
            else
            {
                StartCoroutine(ExitDialogueMode());
            }
        }
        
        private IEnumerator DisplayLine(string line)
        {
            //empty the dialogue text
            dialogueText.text = "";
            //hide items while text is typing
            continueIcon.SetActive(false);
            HideChoices();
                
            canContinueToNextLine = false;

            bool isAddingRichTextTag = false;
            
            // display each letter one at a time
            foreach (char letter in line.ToCharArray())
            {
                // if the submmit button i spressed finish up displaying th eline right away
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    dialogueText.text = line;
                    break;
                }

                // check for rich text tag, if found, add it without waiting
                if (letter == '<' || isAddingRichTextTag) 
                {
                    isAddingRichTextTag = true;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }
                    dialogueText.text += letter;
                }
                // IF NOT RICH TEXT, ADD THE NEXT LETTER AND WAIT A SMALL TIME
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
            //loop through each tag and handle it accordingly
            foreach (string tag in currentTags)
            {
                // parse the tag
                string[] splitTag = tag.Split(':');
                if (splitTag.Length != 2)
                {
                    Debug.LogWarning("Tags are not in correct format: " + tag);
                }
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();
                
                //handle the tag 
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
                    default:
                        Debug.LogWarning("Tag came in but is not currently begin handled: " + tag);
                        break;
                }
            }
        }

        private void DisplayChoices()
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            if (currentChoices.Count > choices.Length)
            {
                Debug.LogError("more choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
            }

            int index = 0;
            //enable and initialize the choices up to the amount of choices for this line of dialogue
            foreach (Choice choice in currentChoices)
            {
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = choice.text;
                index++;
            }   
            //go through the remaining choices the UI supports and make sure they're hidden
            for (int i = index; i < choices.Length; i++)
            {
                choices[i].gameObject.SetActive(false);
            }    
            
            StartCoroutine(SelectFirstChoice());

        }

        private IEnumerator SelectFirstChoice()
        {
            // Event System requires we clear it first, then wait
            // for at Least one frame before we set the current selected object
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
