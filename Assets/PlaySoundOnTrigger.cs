using UnityEngine;
using System.Collections;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 1f;
    public float maxVolume = 0.3f;

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;
            StartCoroutine(FadeInAndPlay());
        }
    }

    IEnumerator FadeInAndPlay()
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, maxVolume, time / fadeDuration);
            yield return null;
        }

        audioSource.volume = maxVolume;
    }
}