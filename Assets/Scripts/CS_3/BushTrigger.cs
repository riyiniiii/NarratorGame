using UnityEngine;
using System.Collections;

public class BushTrigger : MonoBehaviour
{
    private AudioManager audioManager;
    private bool hasPlayed = false;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(PlayDelayedSound());
        }
    }

    private IEnumerator PlayDelayedSound()
    {
        yield return new WaitForSeconds(0.5f);
        audioManager.PlaySFX(audioManager.growling);
    }
}