using UnityEngine;
using System.Collections;

public class MoveSystem : MonoBehaviour
{
    public GameObject correctForm;
    public Camera targetCamera; // assign in the Inspector

    private bool moving;
    private bool finish;

    private Vector3 offset;
    private Vector3 resetPosition;
    private float cachedZ;

    void Start()
    {
        resetPosition = transform.position;

        // Fallback to Camera.main if nothing is assigned
        if (targetCamera == null)
            targetCamera = Camera.main;
        
        StartCoroutine(CaptureResetPosition());
    }

    IEnumerator CaptureResetPosition()
    {
        yield return new WaitForEndOfFrame();
        resetPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (finish) return;

        cachedZ = targetCamera.WorldToScreenPoint(transform.position).z;

        Vector3 mouseWorld = targetCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, cachedZ)
        );
        mouseWorld.z = 0f;

        offset = transform.position - mouseWorld;
        moving = true;
    }

    void Update()
    {
        if (!moving || finish) return;

        Vector3 mouseWorld = targetCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, cachedZ)
        );
        mouseWorld.z = 0f;

        transform.position = mouseWorld + offset;
    }

    void OnMouseUp()
    {
        moving = false;

        if (Vector2.Distance(transform.position, correctForm.transform.position) <= 0.5f)
        {
            transform.position = correctForm.transform.position;
            finish = true;

            GameObject.Find("PointsHandler")
                .GetComponent<WinScript>()
                .AddPoints();
        }
        else
        {
            transform.position = resetPosition;
        }
    }
}