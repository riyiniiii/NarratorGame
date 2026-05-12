using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    public GameObject correctForm;

    private bool moving;
    private bool finish;

    private Vector3 offset;
    private Vector3 resetPosition;

    void Start()
    {
        resetPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (finish) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(transform.position).z)
        );
        mouseWorld.z = 0f;

        offset = transform.position - mouseWorld; // <- this was missing
        moving = true;
    }

    void Update()
    {
        if (!moving || finish) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(transform.position).z)
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