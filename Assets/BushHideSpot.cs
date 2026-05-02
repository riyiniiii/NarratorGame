using UnityEngine;

public class BushHide : MonoBehaviour
{
    private bool playerInRange = false;
    private CharacterController2D player;
    private bool hasBeenUsed = false;

    public WolfHorrorEvent horrorEvent;
    public GameObject hidePrompt;

    private void Update()
    {
        if (hasBeenUsed) return;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            player.SetHidden(true);

            hasBeenUsed = true;

            if (hidePrompt != null)
                hidePrompt.SetActive(false);

            horrorEvent.TriggerEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenUsed) return;

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<CharacterController2D>();

            if (hidePrompt != null)
                hidePrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (hidePrompt != null)
                hidePrompt.SetActive(false);
        }
    }
}  