using UnityEngine;
using System.Collections;

public class WolfReencounterEvent : MonoBehaviour
{
    public CharacterController2D player;
    public Transform wolf;
    public Transform wolfStartPoint;
    public AudioSource breathingSource;

    public TextAsset inkJSON;

    private bool triggered = false;

    public void StartReencounter()
    {
        if (triggered) return;
        StartCoroutine(ReencounterSequence());
    }

    IEnumerator ReencounterSequence()
    {
        triggered = true;

        // ️ freeze player
        player.canMove = false;

        yield return new WaitForSeconds(0.5f);

        //  BREATHING START (tension cue BEFORE reveal)
        breathingSource.volume = 0f;
        breathingSource.Play();

        float audioT = 0f;
        while (audioT < 1f)
        {
            audioT += Time.deltaTime;
            breathingSource.volume = Mathf.Lerp(0f, 1f, audioT);
            yield return null;
        }

        // pause after breathing starts (build tension)
        yield return new WaitForSeconds(1.5f);

        //  WOLF APPEARS AFTER BREATHING
        wolf.gameObject.SetActive(true);
        wolf.position = wolfStartPoint.position;

        SpriteRenderer sr = wolf.GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(1f);

        //  slow creepy step forward
        Vector3 start = wolf.position;
        Vector3 target = wolf.position + Vector3.left * 1f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            wolf.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //  dialogue
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);

        yield return new WaitForSeconds(2f);

        //  fade wolf out
        yield return StartCoroutine(FadeOut(wolf));

        //  restore player
        player.canMove = true;
    }

    IEnumerator FadeOut(Transform wolf)
    {
        SpriteRenderer sr = wolf.GetComponent<SpriteRenderer>();

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t);
            sr.color = new Color(1f, 1f, 1f, a);
            yield return null;
        }

        sr.enabled = false;
    }
}