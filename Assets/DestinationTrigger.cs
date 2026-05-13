using UnityEngine;

public class DestinationTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";
    public bool fadeOnStop = true;
    public float fadeDuration = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (fadeOnStop)
            ChaseAudioManager.Instance.StopChaseAudioFade(fadeDuration);
        else
            ChaseAudioManager.Instance.StopChaseAudio();
    }
}