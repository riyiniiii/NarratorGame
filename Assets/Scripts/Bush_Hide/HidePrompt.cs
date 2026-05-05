using UnityEngine;

public class HidePrompt : MonoBehaviour
{
    public float speed = 2f;
    public float height = 0.1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = startPos + Vector3.up * Mathf.Sin(Time.time * speed) * height;
    }
}