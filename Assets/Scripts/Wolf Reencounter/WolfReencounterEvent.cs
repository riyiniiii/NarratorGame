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

        // freeze player
        player.canMove = false;

        yield return new WaitForSeconds(0.5f);

        // BREATHING START
        breathingSource.volume = 0f;
        breathingSource.Play();

        float audioT = 0f;
        while (audioT < 1f)
        {
            audioT += Time.deltaTime;
            breathingSource.volume = Mathf.Lerp(0f, 1f, audioT);
            yield return null;
        }

        // pause after breathing starts
        yield return new WaitForSeconds(1.5f);

        // WOLF APPEARS
        wolf.gameObject.SetActive(true);
        wolf.position = wolfStartPoint.position;

        SpriteRenderer sr = wolf.GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(1f);

        // slow creepy step forward
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

        // disable stalking logic so look-detection doesn't interfere
        WolfStalking wolfStalking = wolf.GetComponent<WolfStalking>();
        if (wolfStalking != null) wolfStalking.SetEventMode();

        // start dialogue
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);

        // wait for dialogue to fully finish
        yield return new WaitUntil(() => !DialogueManager.GetInstance().dialogueIsPlaying);

        // fade wolf out AFTER dialogue ends
        yield return StartCoroutine(FadeOut(wolf));

        // restore player
        player.canMove = true;
    }

    IEnumerator FadeOut(Transform wolfTransform)
    {
        SpriteRenderer sr = wolfTransform.GetComponent<SpriteRenderer>();

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t);
            sr.color = new Color(1f, 1f, 1f, a);
            yield return null;
        }

        sr.color = new Color(1f, 1f, 1f, 0f);
        sr.enabled = false;
    }
}