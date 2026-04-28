using UnityEngine;

public class BushHide : MonoBehaviour
{
    private bool playerInRange = false;
    private CharacterController2D player;

    public GameObject hidePrompt;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            player.SetHidden(true);
            hidePrompt.SetActive(false);

            Debug.Log("Player hidden");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<CharacterController2D>();

            hidePrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            hidePrompt.SetActive(false);
        }
    }
}