using UnityEngine;

public class HorrorTriggerZone : MonoBehaviour
{
    public WolfHorrorEvent eventController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER TRIGGERED EVENT");
        }
    }
}