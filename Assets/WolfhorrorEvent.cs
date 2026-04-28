using UnityEngine;
using System.Collections;

public class WolfHorrorEvent : MonoBehaviour
{
    public CharacterController2D player;
    public Transform wolf;
    public TextAsset inkJSON;

    public float wolfSpeed = 3f;

    public void StartHideCountdown()
    {
        StartCoroutine(HideCountdown());
    }

    IEnumerator HideCountdown()
    {
        yield return new WaitForSeconds(5f);

        if (player.isHidden)
        {
            Debug.Log("Player hid in time");
            StartCoroutine(HorrorSequence());
        }
        else
        {
            Debug.Log("Player failed to hide!");

            // Put jumpscare or chase logic here
        }
    }

    IEnumerator HorrorSequence()
    {
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.enabled = false;

        Camera.main.transform.position = player.transform.position + new Vector3(0, 0, -10);

        yield return new WaitForSeconds(1f);

        Vector3 startPos = player.transform.position + new Vector3(8f, 0, 0);
        Vector3 endPos = player.transform.position + new Vector3(-8f, 0, 0);

        wolf.position = startPos;

        while (Vector2.Distance(wolf.position, endPos) > 0.1f)
        {
            wolf.position = Vector2.MoveTowards(
                wolf.position,
                endPos,
                wolfSpeed * Time.deltaTime
            );

            yield return null;
        }

        if (inkJSON != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);

            yield return new WaitUntil(() => !DialogueManager.GetInstance().dialogueIsPlaying);
        }

        EndEvent();
    }

    public void EndEvent()
    {
        player.SetHidden(false);

        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.enabled = true;

        Debug.Log("Event ended. Player can move again.");
    }
}