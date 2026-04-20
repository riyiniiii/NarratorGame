using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class CSE_PopUpDialogue : CutsceneElementBase
{
    [SerializeField] private TMP_Text popUpText;
    [TextArea] [SerializeField] private string dialogue;

    [SerializeField] private Animator anim;
    
    private bool isTextActive;

    public override void Execute()
    {
        anim.Play("FadeIn");
        isTextActive = true;
        popUpText.text = dialogue;
    }
    private void Update()
    {
     if (Input.GetKeyDown("Interact") && !isTextActive)
         anim.Play("FadeOut");
    }
}
