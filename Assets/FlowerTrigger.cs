using UnityEngine;

public class FlowerTrigger : MonoBehaviour
{
    public WolfStalking wolf;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wolf.ActivateStalking();
            Destroy(gameObject);
        }
    }
}