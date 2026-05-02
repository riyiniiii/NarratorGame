using UnityEngine;

public class WolfVanishTrigger : MonoBehaviour
{
    public WolfStalking wolf;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (wolf == null)
        {
            Debug.LogError("Wolf not assigned in WolfVanishTrigger!");
            return;
        }

        Debug.Log("Wolf vanish trigger activated");

        wolf.ForceVanish();
    }
}