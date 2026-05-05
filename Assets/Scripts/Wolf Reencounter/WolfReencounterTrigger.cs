using UnityEngine;

public class WolfReencounterTrigger : MonoBehaviour
{
    public WolfReencounterEvent reencounterEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        reencounterEvent.StartReencounter();
    }
}