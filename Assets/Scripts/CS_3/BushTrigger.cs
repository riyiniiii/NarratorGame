using UnityEngine;

    public class BushTrigger : MonoBehaviour
    {
        private AudioManager audioManager;

        private void Start()
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                audioManager.PlaySFX(audioManager.growling);
            }
        }
    }
