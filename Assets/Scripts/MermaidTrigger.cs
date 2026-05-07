using UnityEngine;

public class MermaidTrigger : MonoBehaviour
{
    public MermaidCutscene cutscene;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cutscene.StartCutscene();
        }
    }
}