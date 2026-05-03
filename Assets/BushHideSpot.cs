using UnityEngine;

public class BushHide : MonoBehaviour
{
    private bool playerInRange = false;
    private CharacterController2D player;
    private bool hasBeenUsed = false;

    public WolfHorrorEvent horrorEvent;
    public GameObject hidePrompt;

    private void Start()
    {
        if (hidePrompt != null)
            hidePrompt.SetActive(false);
    }

    private void Update()
    {
        if (hasBeenUsed) return;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("PLAYER HIDES");

            player.SetHidden(true);
            hasBeenUsed = true;

            hidePrompt.SetActive(false);

            horrorEvent.TriggerEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenUsed) return;

        if (!other.CompareTag("Player")) return;

        Debug.Log("ENTER → SHOW PROMPT");

        playerInRange = true;
        player = other.GetComponent<CharacterController2D>();

        if (hidePrompt != null)
            hidePrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("EXIT → HIDE PROMPT");

        playerInRange = false;

        if (hidePrompt != null)
            hidePrompt.SetActive(false);
    }
}