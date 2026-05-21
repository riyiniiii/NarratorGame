using UnityEngine;
using System.Collections;

public class WolfHorrorEvent : MonoBehaviour
{
    public CharacterController2D player;
    public WolfStalking wolf;
    public Transform bushHidePoint;
    public TextAsset inkJSON;

    public float approachSpeed = 2f;
    public float stopDistance = 0.1f;
    public float spawnDistanceRight = 4f;
    public float bushOffsetLeft = 1.5f;

    private bool eventActive;
    private bool originalFacingRight; // store player's original facing direction

    public void TriggerEvent()
    {
        if (eventActive) return;

        StartCoroutine(HorrorSequence());
    }

    private IEnumerator HorrorSequence()
    {
        eventActive = true;

        // freeze player
        player.canMove = false;

        // store original facing direction, then force player to face left
        SpriteRenderer playerSR = player.GetComponent<SpriteRenderer>();
        originalFacingRight = !playerSR.flipX; // true = was facing right
        playerSR.flipX = true;                 // face left

        // reset wolf visuals/state
        wolf.ResetForHorrorEvent();

        yield return new WaitForSeconds(0.5f);

        // spawn wolf to the right of player
        Vector3 spawnPos = player.transform.position + Vector3.right * spawnDistanceRight;
        spawnPos.y = wolf.transform.position.y; // lock Y
        wolf.transform.position = spawnPos;

        yield return new WaitForSeconds(0.5f);

        // target position: slightly left of bush (HORIZONTAL ONLY)
        Vector3 targetPos = new Vector3(
            bushHidePoint.position.x - bushOffsetLeft,
            wolf.transform.position.y,
            wolf.transform.position.z
        );

        // move wolf in straight line (no vertical drift)
        while (Vector2.Distance(
            new Vector2(wolf.transform.position.x, 0),
            new Vector2(targetPos.x, 0)
        ) > stopDistance)
        {
            Vector3 pos = Vector2.MoveTowards(
                wolf.transform.position,
                targetPos,
                approachSpeed * Time.deltaTime
            );

            // force Y to stay fixed
            pos.y = wolf.transform.position.y;
            wolf.transform.position = pos;

            yield return null;
        }

        // ensure wolf is visible
        SpriteRenderer sr = wolf.GetComponent<SpriteRenderer>();
        sr.enabled = true;

        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        // start dialogue while wolf is present
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);

        // wait for dialogue to finish
        while (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            yield return null;
        }

        // fade wolf out after dialogue
        yield return StartCoroutine(VanishWolf(sr));
    }

    private IEnumerator VanishWolf(SpriteRenderer sr)
    {
        float fadeSpeed = 2f;

        while (sr.color.a > 0.05f)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * fadeSpeed);
            sr.color = c;

            yield return null;
        }

        sr.enabled = false;

        // restore player's original facing direction
        SpriteRenderer playerSR = player.GetComponent<SpriteRenderer>();
        playerSR.flipX = !originalFacingRight;

        eventActive = false;
        player.canMove = true;
        player.SetHidden(false);
    }
}