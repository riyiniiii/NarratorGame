using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class ZoomTrigger : MonoBehaviour
{
    [Header("Camera Settings")]
    public CinemachineCamera vcam;
    public float zoomInSize = 2.5f;
    public float normalSize = 5f;
    public float zoomDuration = 1f;

    [Header("Dialogue Settings")]
    public TextAsset inkJSON;

    private DialogueManager dialogueManager;
    private bool triggered = false;

    private void Start()
    {
        dialogueManager = DialogueManager.GetInstance();

        if (dialogueManager == null)
        {
            Debug.LogError("No DialogueManager found!");
        }

        if (vcam == null)
        {
            Debug.LogError("CinemachineCamera is not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Debug.Log("Player entered trigger");
            triggered = true;
            StartCoroutine(CutsceneSequence(other.gameObject));
        }
    }

    private IEnumerator CutsceneSequence(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Freeze player
        if (rb != null)
            rb.simulated = false;

        Debug.Log("Zooming in");

        // Zoom in
        yield return StartCoroutine(SmoothZoom(zoomInSize));

        Debug.Log("Starting dialogue");

        // Start dialogue
        if (dialogueManager != null && inkJSON != null)
        {
            dialogueManager.EnterDialogueMode(inkJSON);
        }
        else
        {
            Debug.LogError("DialogueManager or Ink JSON missing!");
        }

        // Wait for dialogue to finish
        while (dialogueManager != null && dialogueManager.dialogueIsPlaying)
        {
            yield return null;
        }

        Debug.Log("Zooming out");

        // Zoom out
        yield return StartCoroutine(SmoothZoom(normalSize));

        // Unfreeze player
        if (rb != null)
            rb.simulated = true;

        Debug.Log("Cutscene finished");
    }

    private IEnumerator SmoothZoom(float targetSize)
    {
        float startSize = vcam.Lens.OrthographicSize;
        float time = 0f;

        while (time < zoomDuration)
        {
            vcam.Lens.OrthographicSize =
                Mathf.Lerp(startSize, targetSize, time / zoomDuration);

            time += Time.deltaTime;
            yield return null;
        }

        vcam.Lens.OrthographicSize = targetSize;
    }
}