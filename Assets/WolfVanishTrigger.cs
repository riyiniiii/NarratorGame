using UnityEngine;

public class WolfVanishTrigger : MonoBehaviour
{
    public WolfStalking wolf;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        wolf.ResetForHorrorEvent();
        wolf.ActivateStalking();
    }
}