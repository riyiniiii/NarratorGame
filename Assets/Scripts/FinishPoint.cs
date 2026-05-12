using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private void Awake()
    {
        // If this gets moved to DontDestroyOnLoad, destroy it
        if (gameObject.scene.name == "DontDestroyOnLoad")
        {
            Destroy(gameObject);
            return;
        }
    
        Debug.Log("FinishPoint created in scene: " + gameObject.scene.name);
    }
    
    private void Update()
    {
        Debug.Log("FinishPoint active in scene: " + gameObject.scene.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneController.instance.NextLevel1();
        }
    }
}