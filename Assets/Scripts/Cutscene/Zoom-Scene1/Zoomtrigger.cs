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
    public TextAsset inkJSON;   // Your Ink story file

    private DialogueManager dialogueManager;
    private bool triggered = false;

    private void Start()
    {
        // Automatically find the DialogueManager in the scene
        dialogueManager = DialogueManager.GetInstance();

        if (dialogueManager == null)
        {
            Debug.LogError("No DialogueManager found in the scene!");
        }

        BoxCollider2D box = GetComponent<BoxCollider2D>();

        Collider2D player = Physics2D.OverlapBox(
            box.bounds.center,
            box.bounds.size,
            0f
        );

        if (player != null && player.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(CutsceneSequence(player.gameObject));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(CutsceneSequence(other.gameObject));
        }
    }

    private IEnumerator CutsceneSequence(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        yield return StartCoroutine(SmoothZoom(zoomInSize));

        dialogueManager.EnterDialogueMode(inkJSON);

        yield return new WaitUntil(() => !dialogueManager.dialogueIsPlaying);

        yield return StartCoroutine(SmoothZoom(normalSize));

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
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