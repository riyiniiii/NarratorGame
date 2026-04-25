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
        StartCoroutine(ShowPopup());
    }
    private IEnumerator ShowPopup()
    {
        anim.Play("FadeIn");
        popUpText.text = dialogue;

        yield return new WaitForSeconds(3f);

        anim.Play("FadeOut");

        yield return new WaitForSeconds(1f);

        StartCoroutine(WaitAndAdvance());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTextActive)
        {
            anim.Play("FadeOut");
            isTextActive = false;
            StartCoroutine(WaitAndAdvance());
        }
    }
}
