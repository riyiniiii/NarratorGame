using UnityEngine;
using System.Collections;

public class WolfTriggerZone : MonoBehaviour
{
    public WolfHorrorEvent horrorEvent;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("You have 5 seconds to hide!");

            horrorEvent.StartHideCountdown();
        }
    }
}